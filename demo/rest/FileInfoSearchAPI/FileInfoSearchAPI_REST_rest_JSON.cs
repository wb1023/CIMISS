using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cma.cimiss.demo.util;

namespace cma.cimiss.demo.rest.FileInfoSearchAPI
{
    /*
     * REST调取，文件信息列表检索，返回JSON格式
     */
    class FileInfoSearchAPI_REST_rest_JSON
    {
        /*
	   * main方法
	   * 如：按时间段、站号检索雷达文件 getRadaFileByTimeRangeAndStaId
	   */
        static void Main1(string[] args) {
            Console.WriteLine("test API");
            new FileInfoSearchAPI_REST_rest_JSON().test();
        }
        public void test()
        {
	    /* 1. 调用方法的参数定义，并赋值 */
	    string param ="userId=user_nordb" /* 1.1 用户名&密码 */
	        + "&pwd=user_nordb_pwd1"
	        + "&interfaceId=getRadaFileByTimeRangeAndStaId" /* 1.2 接口ID */        
	        + "&dataCode=RADA_L2_UFMT_QC" /* 1.3 必选参数（按需加可选参数） */ //资料：质控前原始格式单站多普勒雷达基数据                                           
	        + "&timeRange=[20140809123000,20140809123600)" //时间段，前闭后开
	        + "&staIds=Z9859,Z9852,Z9856,Z9851,Z9855" //雷达站
	        + "&dataFormat=json"; /* 1.4 序列化格式 */  
	       
	    /* 2. 调用接口 */
	    RestUtil restUtil = new RestUtil() ;
	    string rstData = restUtil.getRestData( param ) ;

	    /* 3.  输出结果 */
	    FormatUtil formatUtil = new FormatUtil() ;
	    formatUtil.outputRstJson( rstData ) ;
        Console.ReadKey();
	  }
    }
}
