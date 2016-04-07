using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cma.cimiss.client;
using cma.cimiss;
using System.Threading;

namespace cma.cimiss.demo.clib.StaElemStatAPI
{
    class StaElemStatAPI_CLIB_callAPI_to_array2D
    {
        //api 服务器ip，各省根据需要修改为本省API服务器ip
         String apiServerIP = "10.20.76.32";
        //端口，不用修改
         int apiServerPort = 1888;
        static void Main1(string[] args)
        {
            Console.WriteLine("call Test API ");
            //TestsaveAsFile();
            new StaElemStatAPI_CLIB_callAPI_to_array2D().test();
            // TestGetFilelist();
            //TestdownLoadFile();

        }
        //测试调用检索数据到二维数组功能
        public void test()
        {
            /* 1. 定义client对象 */
            DataQueryClient client = new DataQueryClient(apiServerIP, apiServerPort);

            /* 2. 调用方法的参数定义，并赋值 */
            /* 2.1 用户名&密码 */
            String userId = "user_rdb";//bj:nmc_mesis nation:user_rdb
            String pwd = "user_rdb_pwd1";// bj:nmc_mesis040829 nation:user_rdb_pwd1
            /* 2.2 接口ID */
            String interfaceId = "getSurfEleByTime";
            /* 2.3 接口参数，多个参数间无顺序 */
            Dictionary<String, String> params1 = new Dictionary<String, String>();
            // 必选参数
            params1.Add("dataCode", "SURF_CHN_MUL_HOR"); // 资料代码
            params1.Add("elements",
                    "Station_ID_C,PRE_1h,PRS,RHU,VIS,WIN_S_Avg_2mi,WIN_D_Avg_2mi,Q_PRS");// 检索要素：站号、站名、小时降水、气压、相对湿度、能见度、2分钟平均风速、2分钟风向
            params1.Add("times", "20140617000000"); // 检索时间
            // 可选参数
            params1.Add("orderby", "Station_ID_C:ASC"); // 排序：按照站号从小到大
            params1.Add("limitCnt", "10"); //返回最多记录数：10

            /* 2.4 返回文件的描述对象 */

            RetArray2D data2d = new RetArray2D();

            /* 3. 调用接口 */
            try
            {
                // 初始化接口服务连接资源
                client.initResources();
                Console.WriteLine("finally initResources");
                // 调用接口
                int rst = client.callAPI_to_array2D(userId, pwd, interfaceId,params1, data2d);
                util.ClibUtil clibUtil = new util.ClibUtil();
                // 输出结果
                if (rst == 0)
                { // 正常返回
                    clibUtil.outputRst(data2d);
                }
                else
                { // 异常返回
                    Console.WriteLine("[error] StaElemSearchAPI_CLIB_callAPI_to_saveAsFile_XML.");
                    Console.WriteLine("\treturn code: %d. \n", rst);
                    Console.WriteLine("\terror message: %s.\n",
                            data2d.request.errorMessage);
                }
                //等待用户输入enter后结束
                Console.ReadKey();
            }
            catch (Exception e)
            {
                // 异常输出
                Console.WriteLine(e.Message);
                //e.Message();
            }
            finally
            {
                // 释放接口服务连接资源
                client.destroyResources();
            }
        }

    }

}
