using System;
using System.Collections.Generic;
using cma.cimiss.client;
using cma.cimiss.demo.util;
using System.Linq;
using System.Text;

namespace cma.cimiss.demo.clib.GridElemPointSearchAPI
{
    /*
     * 客户端调取，格点要素抽取，存储html格式字符串
     */
    class GridElemPointSearchAPI_CLIB_callAPI_to_serializedStr_HTML
    {
        /* 1. 定义client对象 */
        string serverIP = "10.20.76.32";
        int serverPort = 1888;
        /*
	     * main方法
	     * 如：按起报时间、要素、层次、预报时效范围插值经纬度点的时间序列 getNafpTimeSerialByPoint
	     */
        static void Main1(string[] args) {
            Console.WriteLine("call test API");
            new GridElemPointSearchAPI_CLIB_callAPI_to_serializedStr_HTML().test();
        }
        public void test(){
		/* 1. 定义client对象 */
		DataQueryClient client = new DataQueryClient() ;

		/* 2. 调用方法的参数定义，并赋值 */
		/* 2.1 用户名&密码 */
		string userId = "user_nordb";
		string pwd = "user_nordb_pwd1";
		/* 2.2 接口ID */
		string interfaceId = "getNafpTimeSerialByPoint";
		/* 2.3 接口参数，多个参数间无顺序 */
		Dictionary<string, string> param = new Dictionary<string, string>();
		// 必选参数
		param.Add("dataCode", "NAFP_T639_FOR_FTM_HIGH_NEHE"); // 资料：T639高精度格点产品（冬北半球）
		param.Add("time", "20140716000000"); // 起报时间
		param.Add("minVT", "0"); // 起始预报时效
		param.Add("maxVT", "240"); // 终止预报时效
		param.Add("latLons", "39.8/116.4667,31.2/121.4333"); // 经纬度点，北京（纬度39.8，经度116.4667）、上海（纬度31.2，经度121.4333）
		param.Add("fcstEle", "TEM"); // 预报要素（单个)：气温
		param.Add("fcstLevel", "1000"); // 预报层次（单个)：1000hpa
		// 可选参数
		// param.Add("fcstMember", "1"); //集合预报成员（单个)：从1开始
		/* 2.4 返回文件的格式 */
		string dataFormat = "html";
		/* 2.5 返回字符串 */
		StringBuilder retStr = new StringBuilder();

		/* 3. 调用接口 */
		try {
			// 初始化接口服务连接资源
			client.initResources();
			// 调用接口
			int rst = client.callAPI_to_serializedStr(userId, pwd, interfaceId,
					param, dataFormat, retStr);
			// 输出结果
			if (rst == 0) { // 正常返回
				FormatUtil formatUtil = new FormatUtil();
				formatUtil.outputRstHtml(Convert.ToString(retStr));
			} else { // 异常返回
				Console.WriteLine("[error] StaElemSearchAPI_CLIB_callAPI_to_serializedStr_HTML.");
				Console.Write("\treturn code: {0}. \n", rst);
			}
		} catch (Exception e) {
			// 异常输出
            Console.WriteLine(e.Message);
		} finally {
			// 释放接口服务连接资源
			client.destroyResources();
            Console.ReadKey();
		}
	}
    }
}
