using DinkToPdf;
using System.Text;

namespace WebApp.Command.Commands
{
    public class PdfFile<T>
    {
        public readonly List<T> _list;

        public string FileName => $"{typeof(T).Name}.pdf";

        public string FileType => "application/octet-stream";

        public PdfFile(List<T> list)
        {
            _list = list;
        }

        public MemoryStream Create(T instance)
        {
            var type = typeof(T);
            var sb = new StringBuilder();

            // HTML yapısını başlat
            sb.Append($@"<html>
                    <head>
                        <style>
                            .table {{ border-collapse: collapse; width: 80%; margin: 20px auto; }}
                            .table-striped tr:nth-child(even) {{ background-color: #f2f2f2; }}
                            .text-center {{ text-align: center; }}
                            th, td {{ border: 1px solid black; padding: 8px; }}
                        </style>
                    </head>
                    <body>
                        <div class='text-center'><h1>{type.Name}</h1></div>
                        <table class='table table-striped' align='center'>
                            <thead>
                                <tr>");

            // Tablo başlıklarını ekle (T türünün property'leri)
            foreach (var prop in type.GetProperties())
            {
                sb.Append($"<th>{prop.Name}</th>");
            }

            sb.Append("</tr></thead><tbody>");

            // Property değerlerini tablo satırına ekle
            sb.Append("<tr>");
            foreach (var prop in type.GetProperties())
            {
                var value = prop.GetValue(instance) ?? "-"; // Null ise "-" koy
                sb.Append($"<td>{value}</td>");
            }
            sb.Append("</tr>");

            // HTML yapısını tamamla
            sb.Append("</tbody></table></body></html>");


    //        var doc = new HtmlToPdfDocument()
    //        {
    //            GlobalSettings = {
    //    ColorMode = ColorMode.Color,
    //    Orientation = Orientation.Landscape,
    //    PaperSize = PaperKind.A4Plus,
    //},
    //            Objects = {
    //    new ObjectSettings() {
    //        PagesCount = true,
    //        HtmlContent = sb.ToString(),
    //        WebSettings = { DefaultEncoding = "utf-8",UserStyleSheet=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/lib/bootstrap/dist/css/bootstrap.css") },
    //        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
    //    }
    //}
    //        };

            // String'i MemoryStream'e çevir
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(sb.ToString());
            writer.Flush();
            stream.Position = 0; // Stream'in başından okumaya hazır hale getir

            return stream;
        }
    }
}
