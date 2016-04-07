using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cma.cimiss.client;
using cma.cimiss;
using cma.cimiss.demo.util;

namespace cma.cimiss.demo.clib.StaElemStatAPI
{
    class StaElemStatAPI_CLIB_callAPI_to_serializedStr_TEXT
    {
        /* 1. 定义client对象 */
        String serverIP = "10.20.76.32";
        int serverPort = 1888;
        /*
         * main方法 如：按时间段统计中国地面逐小时降水量 statSurfPre
	  */
        static void Main1(string[] args)
        {
            Console.WriteLine("call Test API ");
            //TestsaveAsFile();
            new StaElemStatAPI_CLIB_callAPI_to_serializedStr_TEXT().test();
            // TestGetFilelist();
            //TestdownLoadFile();

        }
        public void test()
        {
            /* 1. 定义client对象 */
            DataQueryClient client = new DataQueryClient();

            /* 2. 调用方法的参数定义，并赋值 */
            /* 2.1 用户名&密码 */
            String userId = "user_rdb";
            String pwd = "user_rdb_pwd1";
            /* 2.2 接口ID */
            String interfaceId = "statSurfPre";
            /* 2.3 接口参数，多个参数间无顺序 */
            Dictionary<String, String> params1 = new Dictionary<String, String>();
            // 必选参数
            // params1.Add("elements", "Station_ID_C,staName"); //统计分组：站号，站名 TODO
            params1.Add("elements", "Station_ID_C,Station_Name");
            params1.Add("timeRange", "(20140815000000,20140815060000]"); // 时间范围，前开后闭
            // 可选参数
            params1.Add("orderby", "SUM_PRE_1h:desc"); // 排序：按照累计降水从大到小
            // params1.Add("statEleValueRanges", "SUM_PRE_1h:(50,)");
            // //统计结果过滤：累计降水值大于50mm的记录
            // params1.Add("limitCnt", "10") ; //返回最多记录数：10
            // params1.Add("staLevels", "011,012,013") ; //台站级别：国家站（基准站、基本站、一般站）

            /* 2.4 返回文件的格式 */
            String dataFormat = "TEXT";
            /* 2.5 返回字符串 */
            StringBuilder retStr = new StringBuilder();

            /* 3. 调用接口 */
            try
            {
                //初始化接口服务连接资源
                client.initResources();
                //调用接口
                int rst = client.callAPI_to_serializedStr(userId, pwd, interfaceId, params1, dataFormat, retStr);
                //输出结果
                if (rst == 0)
                { //正常返回
                    Console.WriteLine(retStr.ToString());
                    FormatUtil formatUtil = new FormatUtil();
                    formatUtil.outputRstText(retStr.ToString());
                }
                else
                { //异常返回
                    Console.WriteLine("[error] StaElemStatAPI_CLIB_callAPI_to_serializedStr_TEXT.");
                    Console.Write("\treturn code: {0}. \n", rst);
                }
                //等待用户输入enter后结束
                Console.ReadKey();
            }
            catch (Exception e)
            {
                //异常输出
                Console.WriteLine(e.Message);
            }
            finally
            {
                //释放接口服务连接资源
                client.destroyResources();
            }

        }
    }
}
