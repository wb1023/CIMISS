using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cma.cimiss.client;
using cma.cimiss.demo.util;
namespace cma.cimiss.demo.clib.StaElemStatAPI
{
    /*
     * 客户端调取，站点要素统计，存储为kml格式
     * 未能正常运行
     */
    class StaElemStatAPI_CLIB_callAPI_to_saveAsFile_KML
    {
        /* 1. 定义client对象 */
        string serverIP = "10.20.76.32";
        int serverPort = 1888;
        /*
         * main方法 如：按时间段统计中国地面逐小时降水量 statSurfPre
         */
        static void Main1(string[] args) {
            Console.WriteLine("call test API");
            new StaElemStatAPI_CLIB_callAPI_to_saveAsFile_KML().test();
        }
        public void test()
        {
		/* 1. 定义client对象 */
		DataQueryClient client = new DataQueryClient() ;

		/* 2. 调用方法的参数定义，并赋值 */
		/* 2.1 用户名&密码 */
		string userId = "user_rdb";
		string pwd = "user_rdb_pwd";
		/* 2.2 接口ID */
		string interfaceId = "statSurfPre";
		/* 2.3 接口参数，多个参数间无顺序 */
		Dictionary<string, string> param = new Dictionary<string, string>();
		// 必选参数
		// Dictionary("elements", "Station_ID_C,staName"); //统计分组：站号，站名 TODO
		param.Add("elements", "Lat,Lon");//统计分组：纬度、经度、1小时降水量
		param.Add("timeRange", "(20140815000000,20140815060000]"); // 时间范围，前开后闭
		// 可选参数
		param.Add("orderby", "SUM_PRE_1h:desc"); // 排序：按照累计降水从大到小
		// Dictionary("statEleValueRanges", "SUM_PRE_1h:(50,)");
		// //统计结果过滤：累计降水值大于50mm的记录
		// Dictionary("limitCnt", "10") ; //返回最多记录数：10
		// Dictionary("staLevels", "011,012,013") ; //台站级别：国家站（基准站、基本站、一般站）
		 /* 2.4 返回文件的格式 */
	    string dataFormat = "kml-p" ;
	    /* 2.5 文件的本地全路径 */
	    string savePath = "e:/temp/test.kml" ;
	    /* 2.6 返回文件的描述对象 */
	    RetFilesInfo retFilesInfo = new RetFilesInfo() ;
	    
	    /* 3. 调用接口 */
	    try {
	      //初始化接口服务连接资源
	      client.initResources() ;
	      //调用接口
	      int rst = client.callAPI_to_saveAsFile(userId, pwd, interfaceId, param, dataFormat, savePath, retFilesInfo) ;
	      //输出结果
	      if(rst == 0) { //正常返回
	        ClibUtil clibUtil = new ClibUtil() ;
	        clibUtil.outputRst( retFilesInfo ) ;
	      } else { //异常返回
	        Console.WriteLine( "[error] StaElemStatAPI_CLIB_callAPI_to_saveAsFile_KML." ) ;       
	        Console.Write( "\treturn code: {0}. \n", rst ) ;
	        Console.Write( "\terror message: {0}.\n", retFilesInfo.request.errorMessage ) ;
	      }
	    } catch (Exception e) {
	      //异常输出
	      Console.WriteLine(e.Message) ;
	    } finally {
	      //释放接口服务连接资源
	      client.destroyResources() ;
          Console.ReadKey();
	    }
	}
    }
}
