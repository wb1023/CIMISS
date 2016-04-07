﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cma.cimiss.client;
using cma.cimiss.demo.util;

namespace cma.cimiss.demo.clib.StaElemSearchAPI
{
    /*
     * 客户端调取，站点资料检索，返回KML文件并下载到本地，且返回RetFilesInfo对象
     * 未能正常運行
     */
    class StaElemSearchAPI_CLIB_callAPI_to_saveAsFile_KML
    {
        /* 1. 定义client对象 */
        string serverIP = "10.20.76.32";
        int serverPort = 1888;
        /*
	     * main方法，程序入口 如：按时间检索地面数据要素 getSurfEleByTime
	     */
        static void Main1(string[] args) {
            Console.WriteLine("call test API");
            new StaElemSearchAPI_CLIB_callAPI_to_saveAsFile_KML().test();
        }
        public void test()
        {
		/* 1. 定义client对象 */
		DataQueryClient client = new DataQueryClient() ;

		/* 2. 调用方法的参数定义，并赋值 */
		/* 2.1 用户名&密码 */
		string userId = "user_rdb";
		string pwd = "user_rdb_pwd1";
		/* 2.2 接口ID */
		string interfaceId = "getSurfEleByTime";
		/* 2.3 接口参数，多个参数间无顺序 */
		Dictionary<string, string> param = new Dictionary<string, string>();
		// 必选参数
		param.Add("dataCode", "SURF_CHN_MUL_HOR"); // 资料代码
		param.Add("elements","lat,lon,PRE_1h");// 检索要素：纬度、经度、小时降水
		param.Add("times", "20140617000000"); // 检索时间
		
		// 可选参数
		//param.Add("ctLevel", "-10,0,1,5,100"); // 色标分级
		param.Add("eleValueRanges", "PRE_1h:(,100)"); // 检索要素值范围
		//param.Add("orderby", "Station_ID_C:ASC"); // 排序：按照站号从小到大
		param.Add("limitCnt", "10") ; //返回最多记录数：10
		
		/* 2.4 返回文件的格式 */
		string dataFormat = "kml-p";
		/* 2.5 文件的本地全路径 */
		string savePath = "e:/temp/StaElemSearchAPI_CLIB_callAPI_to_saveAsFile_KML.kml";
		/* 2.6 返回文件的描述对象 */
		RetFilesInfo retFilesInfo = new RetFilesInfo();

		/* 3. 调用接口 */
		try {
			// 初始化接口服务连接资源
			client.initResources();
			// 调用接口
			int rst = client.callAPI_to_saveAsFile(userId, pwd, interfaceId,
					param, dataFormat, savePath, retFilesInfo);
			// 输出结果
			if (rst == 0) { // 正常返回
				ClibUtil clibUtil = new ClibUtil();
				clibUtil.outputRst(retFilesInfo);
			} else { // 异常返回
				Console.WriteLine("[error] StaElemSearchAPI_CLIB_callAPI_to_saveAsFile_KML.");
				Console.Write("\treturn code: {0}. \n", rst);
				Console.Write("\terror message: {0}.\n",
						retFilesInfo.request.errorMessage);
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
