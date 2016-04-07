using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cma.cimiss.client;
using cma.cimiss.demo.util;
namespace cma.cimiss.demo.clib.StaInfoSearchAPI
{
    /*
     * 客户端调取，台站信息检索，返回XML格式字符串
     *
     */
    class StaInfoSearchAPI_CLIB_callAPI_to_serializedStr_XML
    {
        /* 1. 定义client对象 */
        string serverIP = "10.20.76.32";
        int serverPort = 1888;
        /*
	     * main方法，程序入口 
	     * 如：按照经纬度范围检索台站信息 getStaInfoinRect
	     */
        static void Main1(string[] args) {
            Console.WriteLine("call test API");
            new StaInfoSearchAPI_CLIB_callAPI_to_serializedStr_XML().test();
        }
        public void test()
        {
		
		/* 1. 定义client对象 */
		DataQueryClient client = new DataQueryClient() ;

		/* 2. 调用方法的参数定义，并赋值 */
		/* 2.1 用户名&密码 */
		string userId = "user_nordb"; // TODO:使用user_rdb报错
		string pwd = "user_nordb_pwd1";
		/* 2.2 接口ID */
		string interfaceId = "getStaInfoInRect";
		/* 2.3 接口参数，多个参数间无顺序 */
		Dictionary<string, string> param = new Dictionary<string, string>();
		// 必选参数
		param.Add("dataCode", "STA_INFO_SURF_CHN"); // 资料代码：中国地名台站信息
		param.Add("elements", "Station_ID_C,Station_Name,Lat,Lon,Alti"); // 检索要素：站号、站名、纬度、经度、高度
		param.Add("minLat", "39"); // 经纬度范围：北京及周边（纬度39-42，经度115-117）
		param.Add("maxLat", "42");
		param.Add("minLon", "115");
		param.Add("maxLon", "117");
		// 可选参数

		/* 2.4 返回文件的格式 */
		string dataFormat = "xml";
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
				formatUtil.outputRstXml(Convert.ToString(retStr));
			} else { // 异常返回
				Console.WriteLine("[error] StaInfoSearchAPI_CLIB_callAPI_to_serializedStr_XML.");
				Console.Write("\treturn code: %d. \n", rst);
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
