using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALISS.Data.Client
{
    [HtmlTargetElement("AssemblyVersion", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AssemblyVersionTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";
            output.Content.Append(GetType().Assembly.GetName().Version.ToString());
        }
    }
}
