﻿using System;
using System.Collections.Generic;
using cma.cimiss.client;
using cma.cimiss.demo.util;
using System.Linq;
using System.Text;

namespace cma.cimiss.demo.clib.FileInfoSearchAPI
{
    /*
    * 客户端调取，使用文件流方式下载文件，并返回RetFilesInfo对象
    */
    class FileInfoSearchAPI_CLIB_callAPI_to_downFileByte
    {
        //String serverIP="10.20.76.32";
        //int serverPort = 1888;
    /*
    * main方法
    * 如：按时间段、站号检索雷达文件 getRadaFileByTimeRangeAndStaId
    */
    static void Main1(string[] args) {
        Console.WriteLine("call Test API ");
        new FileInfoSearchAPI_CLIB_callAPI_to_downFileByte().test();
    }
    public void test()
    {
        /* 1. 定义client对象 */
        DataQueryClient client = new DataQueryClient();
        
        /* 2. 调用方法的参数定义，并赋值 */
        /* 2.1 用户名&密码 */
        string userId = "user_nordb";
        string pwd = "user_nordb_pwd1";
        /* 2.2  接口ID */
        String interfaceId = "getRadaFileByTimeRangeAndStaId";
        /* 2.3  接口参数，多个参数间无顺序 */
        Dictionary<string, string> param = new Dictionary<string, string>();

        //必选参数
        param.Add("dataCode", "RADA_L2_UFMT_QC"); //资料：质控前原始格式单站多普勒雷达基数据
        //    params.put("timeRange", "[20140613123000,20140613123600)"); //时间段，前闭后开  TODO:异常
        param.Add("timeRange", "[20150713123000,20150713123600)"); //时间段，前闭后开
        param.Add("staIds", "Z9859,Z9852,Z9856,Z9851,Z9855"); //雷达站
        //可选参数
        /* 2.4 文件的本地保存目录 */
        string saveDir = "e:/temp";
        /* 2.5 返回文件的描述对象 */
        RetFilesInfo retFilesInfo = new RetFilesInfo();
        //字节流定义，复用
        byte[] bytes = null;
        string fileURL = "";

        /* 3. 调用接口 */
        try
        {
            //初始化接口服务连接资源
            client.initResources();
            //调用接口
            int rst = client.callAPI_to_fileList(userId, pwd, interfaceId, param, retFilesInfo);
            //输出结果
            if (rst == 0)
            { //正常返回
                if (retFilesInfo.fileInfos.Length > 0)
                {
                    fileURL = retFilesInfo.fileInfos[0].fileUrl;
                    bytes = client.callAPI_to_downFileByte(fileURL);
                }
                else
                {
                    Console.WriteLine("retFilesInfo is NULL");
                }
            }
            else
            { //异常返回
                Console.WriteLine("[error] FileInfoSearchAPI_CLIB_callAPI_to_downFileByte.");
                Console.WriteLine("\treturn code: {0}. \n", rst);
                Console.WriteLine("\terror message: {0}.\n", retFilesInfo.request.errorMessage);
            }
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
            Console.ReadKey();
        }
    }
    }
}
