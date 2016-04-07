using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cma.cimiss.client;
using cma.cimiss.demo.util;
namespace cma.cimiss.demo.clib.StaElemSearchAPI
{
    /*
     * 客户端调取，站点资料检索，返回HTML字符串
     */
    class StaElemSearchAPI_CLIB_callAPI_to_serializedStr_HTML
    {
        /*
	   * main方法，程序入口
	   * 如：按时间检索地面数据要素 getSurfEleByTime
	   */
       static void Main1(string[] args) {
           Console.WriteLine("call test API");
           new StaElemSearchAPI_CLIB_callAPI_to_serializedStr_HTML().test();
       }
       public void test()
       {
	    /* 1. 定义client对象 */
	    string serverIP="10.20.76.32";
		int serverPort=1888;
		/* 1. 定义client对象 */
		DataQueryClient client = new DataQueryClient() ;

	    /* 2. 调用方法的参数定义，并赋值 */
	    /* 2.1 用户名&密码 */
	    string userId = "user_rdb" ;
	    string pwd = "user_rdb_pwd1" ;
	    /* 2.2  接口ID */
	    string interfaceId = "getSurfEleByTime" ;   
	    /* 2.3  接口参数，多个参数间无顺序 */
	    Dictionary<string, string> param = new Dictionary<string, string>();
	    //必选参数
	    param.Add("dataCode", "SURF_CHN_MUL_HOR") ; //资料代码
	    param.Add("elements", "Station_ID_C,PRE_1h,PRS,RHU,VIS,WIN_S_Avg_2mi,WIN_D_Avg_2mi,Q_PRS") ;//检索要素：站号、站名、小时降水、气压、相对湿度、能见度、2分钟平均风速、2分钟风向
	    param.Add("times", "20140617000000") ; //检索时间
	    //可选参数
	    param.Add("orderby", "Station_ID_C:ASC") ; //排序：按照站号从小到大
	    // param.Add("limitCnt", "10") ; //返回最多记录数：10
	    /* 2.4 返回文件的格式 */
	    string dataFormat = "html" ;
	    /* 2.5 返回字符串 */
	    StringBuilder retStr = new StringBuilder() ;
	    
	    /* 3. 调用接口 */
	    try {
	      //初始化接口服务连接资源
	      client.initResources() ;
	      //调用接口
	      int rst = client.callAPI_to_serializedStr(userId, pwd, interfaceId, param, dataFormat, retStr) ;
	      //输出结果
	      if(rst == 0) { //正常返回
	        FormatUtil formatUtil = new FormatUtil() ;
	        formatUtil.outputRstHtml( Convert.ToString(retStr) );
	      } else { //异常返回
	        Console.WriteLine( "[error] StaElemSearchAPI_CLIB_callAPI_to_serializedStr_HTML." ) ;       
	        Console.Write( "\treturn code: {0}. \n", rst ) ;
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
