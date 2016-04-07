using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cma.cimiss.demo.util;

namespace cma.cimiss.demo.rest.StaElemStatAPI
{
    /*
     * Rest 调取，站点资料统计，返回text格式
     * 运行错误
     */
    class StaElemStatAPI_REST_rest_XML
    {
        /*
	     * main方法 
	     * 如：按时间段统计中国地面逐小时降水量 statSurfPre
	     */
        static void Main1(string[] args) {
            Console.WriteLine("test API");
            new StaElemStatAPI_REST_rest_XML().test();
        }
        public void test()
        {
	    /* 1. 调用方法的参数定义，并赋值 */
	    string param ="userId=user_nordb" /* 1.1 用户名&密码 */
	        + "&pwd=user_nordb_pwd1"
	        + "&interfaceId=statSurfPre" /* 1.2 接口ID */        
	        + "&elements=Station_ID_C,lon,lat" //要素：站号、经度、纬度
	        + "&timeRange=(20140815000000,20140815060000]" //检索时间
	        + "&orderby=SUM_PRE_1h:desc" //排序：按照站号从小到大
	        // + "&limitCnt=10" ; //返回最多记录数：10
	        + "&dataFormat=xml" ; /* 1.4 序列化格式 */
	               
	    /* 2. 调用接口 */
	    RestUtil restUtil = new RestUtil() ;
	    string rstData = restUtil.getRestData( param ) ;
	    
	    /* 3.  输出结果 */
	    FormatUtil formatUtil = new FormatUtil() ;
	    formatUtil.outputRstXml( rstData ) ;
        Console.ReadKey();
	  }
    }
}
