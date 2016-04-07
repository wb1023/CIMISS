using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace cma.cimiss.demo.util
{
    class RestUtil
    {
          /* 
   * TODO:REST服务地址，依具体环境设置(如果使用负载均衡地址比如10.20.76.55，端口号为80,可不写端口号)
   */
   const string host = "10.20.76.32:8008";//负载均衡地址：10.20.76.55
   const int timeoutInMilliSeconds =  1000 * 60 * 2 ; //2 MINUTE

  /*
   * REST请求服务，获取数据
   */
  public String getRestData(string param) {
      HttpWebRequest request;
      HttpWebResponse response = null;
      StreamReader reader;
      StringBuilder sbSource=null;
      if (param == null) { throw new ArgumentNullException("param"); }
      try
      {
          /* 建立web请求*/
          request = WebRequest.Create("http://" + host + "/cimiss-web/api?" + param) as HttpWebRequest;
          request.Timeout = timeoutInMilliSeconds;
          // Get response  
          response = request.GetResponse() as HttpWebResponse;
          if (request.HaveResponse == true && response != null)
          {
              // Get the response stream  
              reader = new StreamReader(response.GetResponseStream());

              // Read it into a StringBuilder  
              sbSource = new StringBuilder(reader.ReadToEnd());
          }  
      }
      catch (WebException wex)
      {
          if (wex.Response != null)
          {
              using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
              {
                  Console.WriteLine(
                      "The server returned '{0}' with the status code {1} ({2:d}).",
                      errorResponse.StatusDescription, errorResponse.StatusCode,
                      errorResponse.StatusCode);
              }
          }  
      }
      finally
      {
          if (response != null) { response.Close(); }
      } 

      return Convert.ToString(sbSource);
     }
    }
}
