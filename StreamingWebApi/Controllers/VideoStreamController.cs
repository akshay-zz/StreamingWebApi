using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace StreamingWebApi.Controllers
{
	public class VideoStreamController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetVideoContent()
        {
			var httpResponce = new HttpResponseMessage
			{
				Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)WriteContentToStream)
			};
			return httpResponce;
        }

        public async void WriteContentToStream(Stream outputStream, HttpContent content, TransportContext transportContext)
        {
            //path of file which we have to read//  
            var filePath = HttpContext.Current.Server.MapPath("~/file_example_MP4_1920_18MG.mp4");
            //here set the size of buffer, you can set any size  
            int bufferSize = 1000;
            byte[] buffer = new byte[bufferSize];
			//here we re using FileStream to read file from server// 
			try
			{
                using(var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    int totalSize = (int)fileStream.Length;
                    
                    while(totalSize > 0)
                    {
                        int count = totalSize > bufferSize ? bufferSize : totalSize;
                        int sizeOfReadedBuffer = fileStream.Read(buffer, 0, count);

                        await outputStream.WriteAsync(buffer, 0, sizeOfReadedBuffer);
                        totalSize -= sizeOfReadedBuffer;
                    }
                }
            }
			catch(Exception ex)
			{

				
			}
           
        }
    }
}