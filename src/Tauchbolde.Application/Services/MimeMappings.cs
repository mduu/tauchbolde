namespace Tauchbolde.Application.Services
{
    public interface IMimeMapping
    {
        string GetMimeMapping(string fileExtension);
        string GetFileExtensionMapping(string mimeType);
    }

    public class MimeMapping: IMimeMapping
    {
        private readonly Dictionary<string, string> extensionMap = new Dictionary<string, string>();

        public MimeMapping()
        {
            extensionMap.Add(".323", "text/h323");
            extensionMap.Add(".asx", "video/x-ms-asf");
            extensionMap.Add(".acx", "application/internet-property-stream");
            extensionMap.Add(".ai", "application/postscript");
            extensionMap.Add(".aif", "audio/x-aiff");
            extensionMap.Add(".aiff", "audio/aiff");
            extensionMap.Add(".axs", "application/olescript");
            extensionMap.Add(".aifc", "audio/aiff");
            extensionMap.Add(".asr", "video/x-ms-asf");
            extensionMap.Add(".avi", "video/x-msvideo");
            extensionMap.Add(".asf", "video/x-ms-asf");
            extensionMap.Add(".au", "audio/basic");
            extensionMap.Add(".application", "application/x-ms-application");
            extensionMap.Add(".bin", "application/octet-stream");
            extensionMap.Add(".bas", "text/plain");
            extensionMap.Add(".bcpio", "application/x-bcpio");
            extensionMap.Add(".bmp", "image/bmp");
            extensionMap.Add(".cdf", "application/x-cdf");
            extensionMap.Add(".cat", "application/vndms-pkiseccat");
            extensionMap.Add(".crt", "application/x-x509-ca-cert");
            extensionMap.Add(".c", "text/plain");
            extensionMap.Add(".css", "text/css");
            extensionMap.Add(".cer", "application/x-x509-ca-cert");
            extensionMap.Add(".crl", "application/pkix-crl");
            extensionMap.Add(".cmx", "image/x-cmx");
            extensionMap.Add(".csh", "application/x-csh");
            extensionMap.Add(".cod", "image/cis-cod");
            extensionMap.Add(".cpio", "application/x-cpio");
            extensionMap.Add(".clp", "application/x-msclip");
            extensionMap.Add(".crd", "application/x-mscardfile");
            extensionMap.Add(".deploy", "application/octet-stream");
            extensionMap.Add(".dll", "application/x-msdownload");
            extensionMap.Add(".dot", "application/msword");
            extensionMap.Add(".doc", "application/msword");
            extensionMap.Add(".dvi", "application/x-dvi");
            extensionMap.Add(".dir", "application/x-director");
            extensionMap.Add(".dxr", "application/x-director");
            extensionMap.Add(".der", "application/x-x509-ca-cert");
            extensionMap.Add(".dib", "image/bmp");
            extensionMap.Add(".dcr", "application/x-director");
            extensionMap.Add(".disco", "text/xml");
            extensionMap.Add(".exe", "application/octet-stream");
            extensionMap.Add(".etx", "text/x-setext");
            extensionMap.Add(".evy", "application/envoy");
            extensionMap.Add(".eml", "message/rfc822");
            extensionMap.Add(".eps", "application/postscript");
            extensionMap.Add(".flr", "x-world/x-vrml");
            extensionMap.Add(".fif", "application/fractals");
            extensionMap.Add(".gtar", "application/x-gtar");
            extensionMap.Add(".gif", "image/gif");
            extensionMap.Add(".gz", "application/x-gzip");
            extensionMap.Add(".hta", "application/hta");
            extensionMap.Add(".htc", "text/x-component");
            extensionMap.Add(".htt", "text/webviewhtml");
            extensionMap.Add(".h", "text/plain");
            extensionMap.Add(".hdf", "application/x-hdf");
            extensionMap.Add(".hlp", "application/winhlp");
            extensionMap.Add(".html", "text/html");
            extensionMap.Add(".htm", "text/html");
            extensionMap.Add(".hqx", "application/mac-binhex40");
            extensionMap.Add(".isp", "application/x-internet-signup");
            extensionMap.Add(".iii", "application/x-iphone");
            extensionMap.Add(".ief", "image/ief");
            extensionMap.Add(".ivf", "video/x-ivf");
            extensionMap.Add(".ins", "application/x-internet-signup");
            extensionMap.Add(".ico", "image/x-icon");
            extensionMap.Add(".jpg", "image/jpeg");
            extensionMap.Add(".jfif", "image/pjpeg");
            extensionMap.Add(".jpe", "image/jpeg");
            extensionMap.Add(".jpeg", "image/jpeg");
            extensionMap.Add(".js", "application/x-javascript");
            extensionMap.Add(".lsx", "video/x-la-asf");
            extensionMap.Add(".latex", "application/x-latex");
            extensionMap.Add(".lsf", "video/x-la-asf");
            extensionMap.Add(".manifest", "application/x-ms-manifest");
            extensionMap.Add(".mhtml", "message/rfc822");
            extensionMap.Add(".mny", "application/x-msmoney");
            extensionMap.Add(".mht", "message/rfc822");
            extensionMap.Add(".mid", "audio/mid");
            extensionMap.Add(".mpv2", "video/mpeg");
            extensionMap.Add(".man", "application/x-troff-man");
            extensionMap.Add(".mvb", "application/x-msmediaview");
            extensionMap.Add(".mpeg", "video/mpeg");
            extensionMap.Add(".m3u", "audio/x-mpegurl");
            extensionMap.Add(".mdb", "application/x-msaccess");
            extensionMap.Add(".mpp", "application/vnd.ms-project");
            extensionMap.Add(".m1v", "video/mpeg");
            extensionMap.Add(".mpa", "video/mpeg");
            extensionMap.Add(".me", "application/x-troff-me");
            extensionMap.Add(".m13", "application/x-msmediaview");
            extensionMap.Add(".movie", "video/x-sgi-movie");
            extensionMap.Add(".m14", "application/x-msmediaview");
            extensionMap.Add(".mpe", "video/mpeg");
            extensionMap.Add(".mp2", "video/mpeg");
            extensionMap.Add(".mov", "video/quicktime");
            extensionMap.Add(".mp3", "audio/mpeg");
            extensionMap.Add(".mpg", "video/mpeg");
            extensionMap.Add(".ms", "application/x-troff-ms");
            extensionMap.Add(".nc", "application/x-netcdf");
            extensionMap.Add(".nws", "message/rfc822");
            extensionMap.Add(".oda", "application/oda");
            extensionMap.Add(".ods", "application/oleobject");
            extensionMap.Add(".pmc", "application/x-perfmon");
            extensionMap.Add(".p7r", "application/x-pkcs7-certreqresp");
            extensionMap.Add(".p7b", "application/x-pkcs7-certificates");
            extensionMap.Add(".p7s", "application/pkcs7-signature");
            extensionMap.Add(".pmw", "application/x-perfmon");
            extensionMap.Add(".ps", "application/postscript");
            extensionMap.Add(".p7c", "application/pkcs7-mime");
            extensionMap.Add(".pbm", "image/x-portable-bitmap");
            extensionMap.Add(".ppm", "image/x-portable-pixmap");
            extensionMap.Add(".pub", "application/x-mspublisher");
            extensionMap.Add(".pnm", "image/x-portable-anymap");
            extensionMap.Add(".pml", "application/x-perfmon");
            extensionMap.Add(".p10", "application/pkcs10");
            extensionMap.Add(".pfx", "application/x-pkcs12");
            extensionMap.Add(".p12", "application/x-pkcs12");
            extensionMap.Add(".pdf", "application/pdf");
            extensionMap.Add(".pps", "application/vnd.ms-powerpoint");
            extensionMap.Add(".p7m", "application/pkcs7-mime");
            extensionMap.Add(".pko", "application/vndms-pkipko");
            extensionMap.Add(".ppt", "application/vnd.ms-powerpoint");
            extensionMap.Add(".pmr", "application/x-perfmon");
            extensionMap.Add(".pma", "application/x-perfmon");
            extensionMap.Add(".pot", "application/vnd.ms-powerpoint");
            extensionMap.Add(".prf", "application/pics-rules");
            extensionMap.Add(".pgm", "image/x-portable-graymap");
            extensionMap.Add(".qt", "video/quicktime");
            extensionMap.Add(".ra", "audio/x-pn-realaudio");
            extensionMap.Add(".rgb", "image/x-rgb");
            extensionMap.Add(".ram", "audio/x-pn-realaudio");
            extensionMap.Add(".rmi", "audio/mid");
            extensionMap.Add(".ras", "image/x-cmu-raster");
            extensionMap.Add(".roff", "application/x-troff");
            extensionMap.Add(".rtf", "application/rtf");
            extensionMap.Add(".rtx", "text/richtext");
            extensionMap.Add(".sv4crc", "application/x-sv4crc");
            extensionMap.Add(".spc", "application/x-pkcs7-certificates");
            extensionMap.Add(".setreg", "application/set-registration-initiation");
            extensionMap.Add(".snd", "audio/basic");
            extensionMap.Add(".stl", "application/vndms-pkistl");
            extensionMap.Add(".setpay", "application/set-payment-initiation");
            extensionMap.Add(".stm", "text/html");
            extensionMap.Add(".shar", "application/x-shar");
            extensionMap.Add(".sh", "application/x-sh");
            extensionMap.Add(".sit", "application/x-stuffit");
            extensionMap.Add(".spl", "application/futuresplash");
            extensionMap.Add(".sct", "text/scriptlet");
            extensionMap.Add(".scd", "application/x-msschedule");
            extensionMap.Add(".sst", "application/vndms-pkicertstore");
            extensionMap.Add(".src", "application/x-wais-source");
            extensionMap.Add(".sv4cpio", "application/x-sv4cpio");
            extensionMap.Add(".tex", "application/x-tex");
            extensionMap.Add(".tgz", "application/x-compressed");
            extensionMap.Add(".t", "application/x-troff");
            extensionMap.Add(".tar", "application/x-tar");
            extensionMap.Add(".tr", "application/x-troff");
            extensionMap.Add(".tif", "image/tiff");
            extensionMap.Add(".txt", "text/plain");
            extensionMap.Add(".texinfo", "application/x-texinfo");
            extensionMap.Add(".trm", "application/x-msterminal");
            extensionMap.Add(".tiff", "image/tiff");
            extensionMap.Add(".tcl", "application/x-tcl");
            extensionMap.Add(".texi", "application/x-texinfo");
            extensionMap.Add(".tsv", "text/tab-separated-values");
            extensionMap.Add(".ustar", "application/x-ustar");
            extensionMap.Add(".uls", "text/iuls");
            extensionMap.Add(".vcf", "text/x-vcard");
            extensionMap.Add(".wps", "application/vnd.ms-works");
            extensionMap.Add(".wav", "audio/wav");
            extensionMap.Add(".wrz", "x-world/x-vrml");
            extensionMap.Add(".wri", "application/x-mswrite");
            extensionMap.Add(".wks", "application/vnd.ms-works");
            extensionMap.Add(".wmf", "application/x-msmetafile");
            extensionMap.Add(".wcm", "application/vnd.ms-works");
            extensionMap.Add(".wrl", "x-world/x-vrml");
            extensionMap.Add(".wdb", "application/vnd.ms-works");
            extensionMap.Add(".wsdl", "text/xml");
            extensionMap.Add(".xml", "text/xml");
            extensionMap.Add(".xlm", "application/vnd.ms-excel");
            extensionMap.Add(".xaf", "x-world/x-vrml");
            extensionMap.Add(".xla", "application/vnd.ms-excel");
            extensionMap.Add(".xls", "application/vnd.ms-excel");
            extensionMap.Add(".xof", "x-world/x-vrml");
            extensionMap.Add(".xlt", "application/vnd.ms-excel");
            extensionMap.Add(".xlc", "application/vnd.ms-excel");
            extensionMap.Add(".xsl", "text/xml");
            extensionMap.Add(".xbm", "image/x-xbitmap");
            extensionMap.Add(".xlw", "application/vnd.ms-excel");
            extensionMap.Add(".xpm", "image/x-xpixmap");
            extensionMap.Add(".xwd", "image/x-xwindowdump");
            extensionMap.Add(".xsd", "text/xml");
            extensionMap.Add(".z", "application/x-compress");
            extensionMap.Add(".zip", "application/x-zip-compressed");
            extensionMap.Add(".*", "application/octet-stream");
        }

        public string GetMimeMapping(string fileExtension)
        {
            fileExtension = fileExtension.ToLower();
            return extensionMap.ContainsKey(fileExtension)
                ? extensionMap[fileExtension]
                : extensionMap[".*"];
        }

        public string GetFileExtensionMapping(string mimeType)
            => extensionMap.FirstOrDefault(i => i.Value.Equals(mimeType, StringComparison.InvariantCultureIgnoreCase)).Key;
    }
}