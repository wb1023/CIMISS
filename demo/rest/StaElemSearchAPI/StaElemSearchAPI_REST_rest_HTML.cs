using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cma.cimiss.demo.util;

namespace cma.cimiss.demo.rest.StaElemSearchAPI
{
    /*
     * rest 调取，站点资料检索，返回HTML 格式
     */
    class StaElemSearchAPI_REST_rest_HTML
    {
        /*
	   * main方法
	   * 如：按时间检索地面数据要素 getSurfEleByTime
	   */
        static void Main1(string[] args) {
            Console.WriteLine("test API");
            new StaElemSearchAPI_REST_rest_HTML().test();
        }
        public void test()
        {
	    /* 1. 调用方法的参数定义，并赋值 */
	    string param ="userId=user_nordb" /* 1.1 用户名&密码 */
	        + "&pwd=user_nordb_pwd1"
	        + "&interfaceId=getSurfEleByTime" /* 1.2 接口ID */        
	        + "&dataCode=SURF_CHN_MUL_HOR" /* 1.3 必选参数（按需加可选参数） */ //资料：中国地面逐小时
	        + "&elements=Station_ID_C,PRE_1h,PRS,RHU,VIS,WIN_S_Avg_2mi,WIN_D_Avg_2mi,Q_PRS" //检索要素：站号、站名、小时降水、气压、相对湿度、能见度、2分钟平均风速、2分钟风向
	        + "&times=20140617000000" //检索时间
	        + "&orderby=Station_ID_C:ASC" //排序：按照站号从小到大
	        // + "&limitCnt=10" ; //返回最多记录数：10
	        + "&dataFormat=html" ; /* 1.4 序列化格式 */
	               
	    /* 2. 调用接口 */
	    RestUtil restUtil = new RestUtil() ;
	    string rstData = restUtil.getRestData( param ) ;
	    
	    /* 3.  输出结果 */
	    FormatUtil formatUtil = new FormatUtil() ;
	    formatUtil.outputRstHtml( rstData ) ;
        Console.ReadKey();
	  }
    }
}
