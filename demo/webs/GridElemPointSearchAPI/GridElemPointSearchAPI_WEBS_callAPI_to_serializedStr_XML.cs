using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cma.cimiss.demo.util;

namespace cma.cimiss.demo.webs.GridElemPointSearchAPI
{
    /*
     * web service调取，返回TEXT格式
     */
    class GridElemPointSearchAPI_WEBS_callAPI_to_serializedStr_XML
    {
        /*
	     * main方法 
	     * 如：按起报时间、要素、层次、预报时效范围插值经纬度点的时间序列 getNafpTimeSerialByPoint
	     */
        static void Main1(string[] args) {
            Console.WriteLine("test API");
            new GridElemPointSearchAPI_WEBS_callAPI_to_serializedStr_XML().test();
        }
        public void test(){
		/* 1. 调用方法的参数定义，并赋值 */
		string param = "userId=user_nordb" /* 1.1 用户名&密码 */
				+ "&pwd=user_nordb_pwd1"
				+ "&interfaceId=getNafpTimeSerialByPoint" /* 1.2 接口ID */
				+ "&dataCode=NAFP_T639_FOR_FTM_HIGH_NEHE" /* 1.3 必选参数（按需加可选参数） */// 资料：质控前原始格式单站多普勒雷达基数据
				+ "&time=20140716000000" // 时间
				+ "&minVT=0"// 起始预报时效
				+ "&maxVT=240"// 终止预报时效
				+ "&latLons=39.8/116.4667,31.2/121.4333"// 经纬度点，北京（纬度39.8，经度116.4667）、上海（纬度31.2，经度121.4333）
				+ "&fcstEle=TEM"// 预报要素（单个)：气温
				+ "&fcstLevel=1000"// 预报层次（单个)：1000hpa
				+ "&dataFormat=xml"; /* 1.4 序列化格式 */

		 /* 2. 调用接口 */
	    WebsUtil websUtil = new WebsUtil() ;
	    string rstData = websUtil.getWsString( "callAPI_to_serializedStr", param ) ;
	    
	    /* 3.  输出结果 */
	    FormatUtil formatUtil = new FormatUtil() ;
	    formatUtil.outputRstXml( rstData ) ;
        Console.ReadKey();
	  }
    }
}
