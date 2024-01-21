
using System;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using System.Collections;

namespace FFF.Clipboard.Test
{
    internal class MyDataSources
    {
        public static IEnumerable ListTextDataFormatWithTypeDataFormat
        {
            get
            {
                foreach(TypeDataFormat item in ListTextDataFormat)
                    foreach (TypeDataFormat itemIn in ListTextDataFormat)
                        yield return new TestCaseData(item, itemIn);
            }
        }

        public static IEnumerable<TypeDataFormat> ListTypeDataFormat =>
            Enum.GetValues(typeof(TypeDataFormat)).Cast<TypeDataFormat>();

        public static IEnumerable<TextDataFormat> ListTextDataFormat =>
            Enum.GetValues(typeof(TextDataFormat)).Cast<TextDataFormat>();

    }
}
