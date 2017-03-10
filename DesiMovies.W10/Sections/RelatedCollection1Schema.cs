using System;
using AppStudio.DataProviders;

namespace DesiMovies.Sections
{
    /// <summary>
    /// Implementation of the RelatedCollection1Schema class.
    /// </summary>
    public class RelatedCollection1Schema : SchemaBase
    {

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string Key { get; set; }
    }
}
