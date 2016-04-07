using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cma.cimiss.demo.util;

namespace cma.cimiss.demo.rest.GridElemRectSearchAPI
{
    /*
     * REST调取，格点场要素获取（切块），返回TEXT字符串
     */
    class GridElemRectSearchAPI_REST_rest_TEXT
    {
        /*
	     * main方法 
	     * 如：按起报时间、要素、层次、预报时效范围插值经纬度点的时间序列 getNafpTimeSerialByPoint
	     */
        static void Main1(string[] args) {
            Console.WriteLine("test API");
            new GridElemRectSearchAPI_REST_rest_TEXT().test();
        }
        public void test()
        {
		/* 1. 调用方法的参数定义，并赋值 */
		string param = "userId=user_nordb" /* 1.1 用户名&密码 */
				+ "&pwd=user_nordb_pwd1"
				+ "&interfaceId=getNafpGridInRect" /* 1.2 接口ID */
				+ "&dataCode=NAFP_T639_FOR_FTM_HIGH_NEHE" /* 1.3 必选参数（按需加可选参数） */// 资料：质控前原始格式单站多普勒雷达基数据
				+ "&time=20140617000000" // 时间
				+ "&validTime=0"// 预报时效：0
				+ "&minLat=39 &maxLat=42 &minLon=115 &maxLon=117"//  经纬度范围：北京及周边（纬度39-42，经度115-117）效
				+ "&fcstEle=TEM"// 预报要素（单个)：气温
				+ "&fcstLevel=1000"// 预报层次（单个)：1000hpa
				+ "&dataFormat=text"; /* 1.4 序列化格式 */

		/* 2. 调用接口 */
		RestUtil restUtil = new RestUtil();
		string rstData = restUtil.getRestData(param);

		/* 3. 输出结果 */
		FormatUtil formatUtil = new FormatUtil();
		formatUtil.outputRstText(rstData);
        Console.ReadKey();
	}
    }
}
