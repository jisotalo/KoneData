/*
MIT License

Copyright (c) 2021 Jussi Isotalo

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KoneData
{
    public class KoneService
    {
        private const string BASE_URL = "https://konedata.net/";

        /// <summary>
        /// Downloads given URL and converts it to right encoding
        /// Automatic encoding detection did not work properly with konedata.net and HtmlAgilityPack's HtmlWeb
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<string> DownloadPage(string url)
        {
            try
            {
                var client = new HttpClient();
                var res = await client.GetAsync(new Uri(url));
                var data = await res.Content.ReadAsByteArrayAsync();

                //Converting to 
                if(res.Content.Headers.GetValues("Content-Type").FirstOrDefault().Contains("UTF-8"))
                {
                    return Encoding.UTF8.GetString(data);
                }

                return Encoding.GetEncoding("ISO-8859-1").GetString(data);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to download page {url}, error: {ex.Message}");
                throw new Exception($"Failed to download page {url}, error: {ex.Message}", ex);
            }
        }




        /// <summary>
        /// Downloads given URL and then returns it as new HtmlDocument
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<HtmlAgilityPack.HtmlDocument> GetPageAsHtmlDoc(string url)
        {
            var source = await DownloadPage(url);

            try
            {
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(source);

                return doc;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load html document, error: {ex.Message}");
                throw new Exception($"Failed to load html document, error: {ex.Message}", ex);
            }
        }





        /// <summary>
        /// Fetches all tractor brands
        /// </summary>
        /// <returns></returns>
        public async Task<List<TractorBrand>> GetTractorBrands()
        {
            var doc = await GetPageAsHtmlDoc(BASE_URL);

            try
            {
                //Find <li> that has <a><span>Traktorit</span></a>
                var container = doc.DocumentNode.SelectSingleNode("//li[./a/span = 'Traktorit']");

                //The <li> contains all brands
                var brands = container.SelectNodes("./ul/li").Select(brand => new TractorBrand
                {
                    Name = WebUtility.HtmlDecode(brand.InnerText).Trim(),
                    Url = brand.SelectSingleNode("./a").Attributes["href"]?.Value
                }).ToList();

                return brands;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to parse tractor brands from html document, error: {ex.Message}");
                throw new Exception($"Failed to parse tractor brands from html document, error: {ex.Message}", ex);
            }
        }




        /// <summary>
        /// Fetches all tractor series from given url (= given brand)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<List<TractorSeries>> GetTractorBrandSeries(string url)
        {
            var doc = await GetPageAsHtmlDoc(url);

            try
            {
                //Find <div class="entry-content">
                var container = doc.DocumentNode.SelectSingleNode("//div[@class=\"entry-content\"]");

                //All series are under it as <p>
                var series = container.SelectNodes("./p[./a]");

                return series.Select(serie => new TractorSeries
                {
                    Name = WebUtility.HtmlDecode(serie.InnerText).Trim(),
                    Url = serie.SelectSingleNode("./a").Attributes["href"]?.Value
                }).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to parse tractor series from html document, error: {ex.Message}");
                throw new Exception($"Failed to parse tractor series from html document, error: {ex.Message}", ex);
            }
        }





        /// <summary>
        /// Fetches all tractor models with their properties from given url (= given tractor series)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<List<TractorModel>> GetTractorModels(string url)
        {
            var doc = await GetPageAsHtmlDoc(url);

            try
            {
                var results = new List<TractorModel>();

                //Get all <tr> under first <table>
                var rows2 = doc.DocumentNode.SelectSingleNode("//table").SelectNodes(".//tr");

                var table = doc.DocumentNode.SelectSingleNode("//table");
                var headerRows = table.SelectNodes(".//tr[./*[self::td or self::th][1][string-length(text()) = 0]]");
                var dataRows = table.SelectNodes(".//tr[./*[self::td or self::th][1][string-length(text()) > 0]]");

                var models = new List<TractorModel>();

                //Models are first header rows that have data combined, until there is an empty row
                foreach(var row in headerRows)
                {
                    var cells = row.SelectNodes("./*[self::td or self::th]");
                    var cellsWithData = cells.Select((item, index) => new { Item = item, Index = index }).Where(pair => WebUtility.HtmlDecode(pair.Item.InnerText).Trim() != "");

                    //Empty row found -> we are done here
                    if(cellsWithData.Count() == 0)
                    {
                        break;
                    }

                    if(models.Count == 0)
                    {
                        //First row (model names)

                        cellsWithData.ToList().ForEach(cell =>
                        {
                            models.Add(new TractorModel
                            {
                                Name = WebUtility.HtmlDecode(cell.Item.InnerText).Trim(),
                                TableColumnIndex = cell.Index
                            });
                        });
                    }
                    else
                    {
                        //Other rows (additional model data)
                        cellsWithData.ToList().ForEach(cell =>
                        {
                            models.First(m => m.TableColumnIndex == cell.Index).Name += $" {cell.Item.InnerText}";
                        });
                    }
                }



                //Properties
                foreach (var row in dataRows)
                {
                    var cells = row.SelectNodes("./td");
                    var cellsWithData = cells.Select((item, index) => new { Item = item, Index = index }).Where(pair => WebUtility.HtmlDecode(pair.Item.InnerText).Trim() != "");

                    //First two cells are always property names

                    var NameFI = WebUtility.HtmlDecode(cellsWithData.First().Item.InnerText).Trim();
                    var NameEN = WebUtility.HtmlDecode(cellsWithData.Skip(1).First().Item.InnerText).Trim();


                    //Start from first table column index
                    cellsWithData.Skip(models.First().TableColumnIndex).ToList().ForEach(cell =>
                    {
                        var model = models.First(m => m.TableColumnIndex == cell.Index);

                        //Some properties (like years) might be multiple times, so skip them
                        if (model.Properties.FirstOrDefault(m => m.NameFI == NameFI) == null)
                        {
                            model.Properties.Add(new TractorProperty
                            {
                                NameFI = NameFI,
                                NameEN = NameEN,
                                Value = WebUtility.HtmlDecode(cell.Item.InnerText).Trim()
                            });
                        }
                    });
                }

                return models;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to parse tractor models from html document, error: {ex.Message}");
                throw new Exception($"Failed to parse tractor modelsfrom html document, error: {ex.Message}", ex);
            }
        }
    }







    /// <summary>
    /// Interface for classes that have URL address
    /// </summary>
    public interface IHasUrlAddr
    {
        public string Url { get; set; }
    }


    /// <summary>
    /// Tractor brand
    /// </summary>
    public class TractorBrand : IHasUrlAddr
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }


    /// <summary>
    /// Trator series
    /// </summary>
    public class TractorSeries : IHasUrlAddr
    {
        public TractorBrand Brand { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }


    /// <summary>
    /// Tractor model
    /// </summary>
    public class TractorModel
    {
        public TractorSeries Series { get; set; }

        public string Name;
        public int TableColumnIndex;
        public List<TractorProperty> Properties = new List<TractorProperty>();
        public override string ToString()
        {
            return Name;
        }
    }


    /// <summary>
    /// Single property of an tractor
    /// </summary>
    public class TractorProperty
    {
        public string NameFI;
        public string NameEN;
        public string Value;
    }
}
