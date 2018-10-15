using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TagHelpers
{
    [HtmlTargetElement("gravatar")]
    public class Gravatar : TagHelper
    {
        private const string EmailAddressAttributeName = "email-address";
        private const string ImageSizeAttributName = "image-size";
        private const string DefaultImageAtributeName = "default-image";
        private const string DefaultImageUrlAtributeName = "default-image-url";
        private const string ForceDefaultImageAtributeName = "force-default-image";
        private const string RatingAtributeName = "rating";
        private const string ForceSecureRequestAtributeName = "force-secure-request";
        private const string AdditionalCssClassesAtributeName = "additional-css-classes";

        [HtmlAttributeName(EmailAddressAttributeName)]
        public string EmailAddress { get; set; }
        [HtmlAttributeName(ImageSizeAttributName)]
        public int ImageSize { get; set; } = 80;
        [HtmlAttributeName(DefaultImageAtributeName)]
        public DefaultImage DefaultImage { get; set; } = DefaultImage.Default;
        [HtmlAttributeName(DefaultImageUrlAtributeName)]
        public string DefaultImageUrl { get; set; } = "";
        [HtmlAttributeName(ForceDefaultImageAtributeName)]
        public bool ForceDefaultImage { get; set; }
        [HtmlAttributeName(RatingAtributeName)]
        public Rating Rating { get; set; } = Rating.G;
        [HtmlAttributeName(ForceSecureRequestAtributeName)]
        public bool ForceSecureRequest { get; set; }
        [HtmlAttributeName(AdditionalCssClassesAtributeName)]
        public string AdditionalCssClasses { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            output.TagName = "img";
            var email = string.IsNullOrWhiteSpace(EmailAddress) ? string.Empty : EmailAddress.ToLower();

            output.Attributes.Add("src",
                string.Format("{0}://{1}.gravatar.com/avatar/{2}?s={3}{4}{5}{6}",
                    "https",
                    "s",
                    GravatarUtilities.GetMd5Hash(email),
                    ImageSize,
                    "&d=" + (!string.IsNullOrEmpty(DefaultImageUrl) ? HtmlEncoder.Default.Encode(DefaultImageUrl) : DefaultImage.GetDescription()),
                    ForceDefaultImage ? "&f=y" : "",
                    "&r=" + Rating.GetDescription()
                )
            );
            if (!string.IsNullOrWhiteSpace(AdditionalCssClasses))
            {
                if (output.Attributes.Any(x => x.Name.ToLower() == "class"))
                {
                    AdditionalCssClasses = output.Attributes.First(x => x.Name.ToLower() == "class").Value + " " + AdditionalCssClasses;
                    output.Attributes.Remove(output.Attributes.First(x => x.Name.ToLower() == "class"));
                }

                // Add the additional CSS classes
                output.Attributes.Add("class", AdditionalCssClasses);
            }
            base.Process(context, output);
        }
    }



    #region Utils

    public static class GravatarUtilities
    {
        /// <summary>
        /// Generates an MD5 hash of the given string
        /// </summary>
        /// <remarks>Source: http://msdn.microsoft.com/en-us/library/system.security.cryptography.md5.aspx </remarks>
        public static string GetMd5Hash(string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }



        /// <summary>
        /// Returns the value of a DescriptionAttribute for a given Enum value
        /// </summary>
        /// <remarks>Source: http://blogs.msdn.com/b/abhinaba/archive/2005/10/21/483337.aspx </remarks>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum en)
        {

            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).ToArray();

                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return en.ToString();

        }
    }

    #endregion


    #region Enums

    /// <summary>
    /// In addition to allowing you to use your own image, Gravatar has a number of built in options which you can also use as defaults. Most of these work by taking the requested email hash and using it to generate a themed image that is unique to that email address
    /// </summary>
    public enum DefaultImage
    {
        /// <summary>Default Gravatar logo</summary>
        [Description("")]
        Default,
        /// <summary>404 - do not load any image if none is associated with the email hash, instead return an HTTP 404 (File Not Found) response</summary>
        [Description("404")]
        Http404,
        /// <summary>Mystery-Man - a simple, cartoon-style silhouetted outline of a person (does not vary by email hash)</summary>
        [Description("mm")]
        MysteryMan,
        /// <summary>Identicon - a geometric pattern based on an email hash</summary>
        [Description("identicon")]
        Identicon,
        /// <summary>MonsterId - a generated 'monster' with different colors, faces, etc</summary>
        [Description("monsterid")]
        MonsterId,
        /// <summary>Wavatar - generated faces with differing features and backgrounds</summary>
        [Description("wavatar")]
        Wavatar,
        /// <summary>Retro - awesome generated, 8-bit arcade-style pixelated faces</summary>
        [Description("retro")]
        Retro
    }

    /// <summary>
    /// Gravatar allows users to self-rate their images so that they can indicate if an image is appropriate for a certain audience. By default, only 'G' rated images are displayed unless you indicate that you would like to see higher ratings
    /// </summary>
    public enum Rating
    {
        /// <summary>Suitable for display on all websites with any audience type</summary>
        [Description("g")]
        G,
        /// <summary>May contain rude gestures, provocatively dressed individuals, the lesser swear words, or mild violence</summary>
        [Description("pg")]
        PG,
        /// <summary>May contain such things as harsh profanity, intense violence, nudity, or hard drug use</summary>
        [Description("r")]
        R,
        /// <summary>May contain hardcore sexual imagery or extremely disturbing violence</summary>
        [Description("x")]
        X
    }
    #endregion
    
}
