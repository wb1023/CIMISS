using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cma.cimiss.demo.util;

namespace cma.cimiss.demo.webs.StaInfoSearchAPI
{
    /*
     * web service调取，站点信息检索，返回HTML字符串
     */
    class StaInfoSearchAPI_WEBS_callAPI_to_serializedStr_HTML
    {
        /*
	     * main方法，程序入口 
	     * 如：按照经纬度范围检索台站信息 getStaInfoinRect
	     */
        static void Main1(string[] args) {
            Console.WriteLine("test API");
            new StaInfoSearchAPI_WEBS_callAPI_to_serializedStr_HTML().test();
        }
        public void test()
        {
		/* 1. 调用方法的参数定义，并赋值 */
		string param = "userId=user_nordb" ///* 1.1 用户名&密码 */
				+ "&pwd=user_nordb_pwd1" 
				+ "&interfaceId=getStaInfoInRect" ///*1.2 接口ID */
				+ "&dataCode=STA_INFO_SURF_CHN" //* 1.3 必选参数*///资料代码 
				+ "&elements=Station_ID_C,Station_Name,Lat,Lon,Alti" // 检索要素：站号、站名、纬度、经度、高度
				+ "&minLat=39&maxLat=42&minLon=115&maxLon=117" // 经纬度范围：北京及周边（纬度39-42，经度115-117）
				/* 可选参数**/
				// + "&orderby=Station_ID_C:desc"  //排序：按照站号从小到大
				// + "&limitCnt=10" ; //返回最多记录数：10
				+ "&dataFormat=html"; /* 1.4 序列化格式 */

		/* 2. 调用接口 */
	    WebsUtil websUtil = new WebsUtil() ;
	    string rstData = websUtil.getWsString( "callAPI_to_serializedStr", param ) ;
	    
	    /* 3.  输出结果 */
	    FormatUtil formatUtil = new FormatUtil() ;
	    formatUtil.outputRstHtml( rstData ) ;
        Console.ReadKey();
	  }

    }
}
