using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SportsStoreUsingCore.TagHelpers
{
    //最终生成：<reverse-text-div></reverse-text-div>
    //这里的定义生成ITagHelper接口，然后交给Razor引擎
    //[HtmlTargetElement("tag-name")]
    public class ReverseTextDivTagHelper:TagHelper
    {
        public string _divData = string.Empty;

        public string DivData
        {
            get
            {
                //字符串转换成字符数组
                char[] reversedData = _divData.ToCharArray();
                Array.Reverse(reversedData);
                String sDataReversed = new String(reversedData);
                return AllCaps ? sDataReversed.ToUpper() : sDataReversed;
            }
            set
            {
                _divData = value;
            }
        }

        public bool AllCaps { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Content.SetContent(DivData);
        }
    }
}
