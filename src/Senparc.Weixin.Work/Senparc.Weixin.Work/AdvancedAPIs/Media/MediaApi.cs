﻿/*----------------------------------------------------------------
    Copyright (C) 2016 Senparc
    
    文件名：MediaApi.cs
    文件功能描述：多媒体文件接口
    
    
    创建标识：Senparc - 20150313
    
    修改标识：Senparc - 20150313
    修改描述：整理接口
 
    修改标识：Senparc - 20150313
    修改描述：开放代理请求超时时间
 
    修改标识：Senparc - 20160720
    修改描述：增加其接口的异步方法
----------------------------------------------------------------*/

/*
    接口详见：http://qydev.weixin.qq.com/wiki/index.php?title=%E7%AE%A1%E7%90%86%E5%A4%9A%E5%AA%92%E4%BD%93%E6%96%87%E4%BB%B6
 */

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Senparc.Weixin.Entities;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.Work.AdvancedAPIs.Media;
using Senparc.Weixin.Work.CommonAPIs;
using Senparc.Weixin.Work.Entities;

namespace Senparc.Weixin.Work.AdvancedAPIs
{
    /// <summary>
    /// 多媒体文件接口
    /// </summary>
    public static class MediaApi
    {
        #region 同步请求

        /// <summary>
        /// 上传临时媒体文件
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video），普通文件(file)</param>
        /// <param name="media">form-data中媒体文件标识，有filename、filelength、content-type等信息</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static UploadTemporaryResultJson Upload(string accessToken, UploadMediaFileType type, string media, int timeOut = Config.TIME_OUT)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", accessToken.AsUrlData(), type.ToString());
            var fileDictionary = new Dictionary<string, string>();
            fileDictionary["media"] = media;
            return Post.PostFileGetJson<UploadTemporaryResultJson>(url, null, fileDictionary, null, null,null, timeOut);
        }

        /// <summary>
        /// 获取临时媒体文件
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mediaId"></param>
        /// <param name="stream"></param>
        public static void Get(string accessToken, string mediaId, Stream stream)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}",
                accessToken.AsUrlData(), mediaId.AsUrlData());
            HttpUtility.Get.Download(url, stream);//todo 异常处理
        }

        /// <summary>
        /// 上传永久图文素材
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="agentId"></param>
        /// <param name="timeOut"></param>
        /// <param name="mpNewsArticles"></param>
        /// <returns></returns>
        public static UploadForeverResultJson AddMpNews(string accessToken, int agentId, int timeOut = Config.TIME_OUT, params MpNewsArticle[] mpNewsArticles)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/add_mpnews?access_token={0}",
                accessToken.AsUrlData());

            var data = new
            {
                agentid = agentId,
                mpnews = new
                {
                    articles = mpNewsArticles
                }
            };

            return CommonJsonSend.Send<UploadForeverResultJson>(null, url, data, CommonJsonSendType.POST, timeOut);
        }

        /// <summary>
        /// 上传其他类型永久素材
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="type"></param>
        /// <param name="agentId"></param>
        /// <param name="media"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static UploadForeverResultJson AddMaterial(string accessToken, UploadMediaFileType type, int agentId, string media, int timeOut = Config.TIME_OUT)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/add_material?agentid={1}&type={2}&access_token={0}", accessToken.AsUrlData(), agentId, type);
            var fileDictionary = new Dictionary<string, string>();
            fileDictionary["media"] = media;
            return Post.PostFileGetJson<UploadForeverResultJson>(url, null, fileDictionary, null, null,null, timeOut);
        }

        /// <summary>
        /// 获取永久图文素材
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="agentId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public static GetForeverMpNewsResult GetForeverMpNews(string accessToken, int agentId, string mediaId)
        {
            var url =
                string.Format(
                    "https://qyapi.weixin.qq.com/cgi-bin/material/get?access_token={0}&media_id={1}&agentid={2}",
                    accessToken.AsUrlData(), mediaId.AsUrlData(), agentId);

            return CommonJsonSend.Send<GetForeverMpNewsResult>(null, url, null, CommonJsonSendType.GET);
        }

        /// <summary>
        /// 获取临时媒体文件
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="agentId"></param>
        /// <param name="mediaId"></param>
        /// <param name="stream"></param>
        public static void GetForeverMaterial(string accessToken, int agentId, string mediaId, Stream stream)
        {
            var url =
                string.Format(
                    "https://qyapi.weixin.qq.com/cgi-bin/material/get?access_token={0}&media_id={1}&agentid={2}",
                    accessToken.AsUrlData(), mediaId.AsUrlData(), agentId);

            HttpUtility.Get.Download(url, stream);//todo 异常处理
        }

        /// <summary>
        /// 删除永久素材
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="agentId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public static QyJsonResult DeleteForeverMaterial(string accessToken, int agentId, string mediaId)
        {
            var url =
                string.Format(
                    "https://qyapi.weixin.qq.com/cgi-bin/material/del?access_token={0}&agentid={1}&media_id={2}",
                    accessToken.AsUrlData(), agentId, mediaId.AsUrlData());

            return CommonJsonSend.Send<QyJsonResult>(null, url, null, CommonJsonSendType.GET);
        }

        /// <summary>
        /// 修改永久图文素材
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mediaId"></param>
        /// <param name="agentId"></param>
        /// <param name="timeOut"></param>
        /// <param name="mpNewsArticles"></param>
        /// <returns></returns>
        public static UploadForeverResultJson UpdateMpNews(string accessToken, string mediaId, int agentId, int timeOut = Config.TIME_OUT, params MpNewsArticle[] mpNewsArticles)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/update_mpnews?access_token={0}",
                accessToken.AsUrlData());

            var data = new
            {
                agentid = agentId,
                media_id = mediaId,
                mpnews = new
                {
                    articles = mpNewsArticles
                }
            };

            return CommonJsonSend.Send<UploadForeverResultJson>(null, url, data, CommonJsonSendType.POST, timeOut);
        }

        /// <summary>
        /// 获取素材总数
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public static GetCountResult GetCount(string accessToken, int agentId)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/get_count?access_token={0}&agentid={1}",
                accessToken.AsUrlData(), agentId);

            return CommonJsonSend.Send<GetCountResult>(null, url, null, CommonJsonSendType.GET);
        }

        /// <summary>
        /// 获取素材列表
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="type"></param>
        /// <param name="agentId"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static BatchGetMaterialResult BatchGetMaterial(string accessToken, UploadMediaFileType type, int agentId, int offset, int count, int timeOut = Config.TIME_OUT)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/batchget?access_token={0}",
                accessToken.AsUrlData());

            var data = new
            {
                type = type.ToString(),
                agentid = agentId,
                offset = offset,
                count = count,
            };

            return CommonJsonSend.Send<BatchGetMaterialResult>(null, url, data, CommonJsonSendType.POST, timeOut);
        }
        /// <summary>
        /// 上传图文消息内的图片
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="media"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static UploadimgMediaResult UploadimgMedia(string accessToken, string media, int timeOut = Config.TIME_OUT)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/uploadimg?access_token={0}",
                accessToken.AsUrlData());

            var data = new
            {
                media = media
            };

            return CommonJsonSend.Send<UploadimgMediaResult>(null, url, data, CommonJsonSendType.POST, timeOut);
        }
        #endregion

        #region 异步请求
        /// <summary>
        /// 【异步方法】上传临时媒体文件
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video），普通文件(file)</param>
        /// <param name="media">form-data中媒体文件标识，有filename、filelength、content-type等信息</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static async Task<UploadTemporaryResultJson> UploadAsync(string accessToken, UploadMediaFileType type, string media, int timeOut = Config.TIME_OUT)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", accessToken.AsUrlData(), type.ToString());
            var fileDictionary = new Dictionary<string, string>();
            fileDictionary["media"] = media;
            return await Post.PostFileGetJsonAsync<UploadTemporaryResultJson>(url, null, fileDictionary, null, null,null, timeOut);
        }

        /// <summary>
        /// 【异步方法】获取临时媒体文件
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mediaId"></param>
        /// <param name="stream"></param>
        public static async Task GetAsync(string accessToken, string mediaId, Stream stream)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}",
                accessToken.AsUrlData(), mediaId.AsUrlData());
            await HttpUtility.Get.DownloadAsync(url, stream);//todo 异常处理
        }

        /// <summary>
        /// 【异步方法】上传永久图文素材
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="agentId"></param>
        /// <param name="timeOut"></param>
        /// <param name="mpNewsArticles"></param>
        /// <returns></returns>
        public static async Task<UploadForeverResultJson> AddMpNewsAsync(string accessToken, int agentId, int timeOut = Config.TIME_OUT, params MpNewsArticle[] mpNewsArticles)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/add_mpnews?access_token={0}",
                accessToken.AsUrlData());

            var data = new
            {
                agentid = agentId,
                mpnews = new
                {
                    articles = mpNewsArticles
                }
            };

            return await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync<UploadForeverResultJson>(null, url, data, CommonJsonSendType.POST, timeOut);
        }

        /// <summary>
        /// 【异步方法】上传其他类型永久素材
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="type"></param>
        /// <param name="agentId"></param>
        /// <param name="media"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static async Task<UploadForeverResultJson> AddMaterialAsync(string accessToken, UploadMediaFileType type, int agentId, string media, int timeOut = Config.TIME_OUT)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/add_material?agentid={1}&type={2}&access_token={0}", accessToken.AsUrlData(), agentId, type);
            var fileDictionary = new Dictionary<string, string>();
            fileDictionary["media"] = media;
            return await Post.PostFileGetJsonAsync<UploadForeverResultJson>(url, null, fileDictionary, null, null,null, timeOut);
        }

        /// <summary>
        /// 【异步方法】获取永久图文素材
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="agentId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public static async Task<GetForeverMpNewsResult> GetForeverMpNewsAsync(string accessToken, int agentId, string mediaId)
        {
            var url =
                string.Format(
                    "https://qyapi.weixin.qq.com/cgi-bin/material/get?access_token={0}&media_id={1}&agentid={2}",
                    accessToken.AsUrlData(), mediaId.AsUrlData(), agentId);

            return await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync<GetForeverMpNewsResult>(null, url, null, CommonJsonSendType.GET);
        }

        /// <summary>
        ///【异步方法】获取临时媒体文件
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="agentId"></param>
        /// <param name="mediaId"></param>
        /// <param name="stream"></param>
        public static async Task GetForeverMaterialAsync(string accessToken, int agentId, string mediaId, Stream stream)
        {
            var url =
                string.Format(
                    "https://qyapi.weixin.qq.com/cgi-bin/material/get?access_token={0}&media_id={1}&agentid={2}",
                    accessToken.AsUrlData(), mediaId.AsUrlData(), agentId);

            await HttpUtility.Get.DownloadAsync(url, stream);//todo 异常处理
        }

        /// <summary>
        /// 【异步方法】删除永久素材
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="agentId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public static async Task<QyJsonResult> DeleteForeverMaterialAsync(string accessToken, int agentId, string mediaId)
        {
            var url =
                string.Format(
                    "https://qyapi.weixin.qq.com/cgi-bin/material/del?access_token={0}&agentid={1}&media_id={2}",
                    accessToken.AsUrlData(), agentId, mediaId.AsUrlData());

            return await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync<QyJsonResult>(null, url, null, CommonJsonSendType.GET);
        }

        /// <summary>
        /// 【异步方法】修改永久图文素材
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mediaId"></param>
        /// <param name="agentId"></param>
        /// <param name="timeOut"></param>
        /// <param name="mpNewsArticles"></param>
        /// <returns></returns>
        public static async Task<UploadForeverResultJson> UpdateMpNewsAsync(string accessToken, string mediaId, int agentId, int timeOut = Config.TIME_OUT, params MpNewsArticle[] mpNewsArticles)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/update_mpnews?access_token={0}",
                accessToken.AsUrlData());

            var data = new
            {
                agentid = agentId,
                media_id = mediaId,
                mpnews = new
                {
                    articles = mpNewsArticles
                }
            };

            return await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync<UploadForeverResultJson>(null, url, data, CommonJsonSendType.POST, timeOut);
        }

        /// <summary>
        /// 【异步方法】获取素材总数
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public static async Task<GetCountResult> GetCountAsync(string accessToken, int agentId)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/get_count?access_token={0}&agentid={1}",
                accessToken.AsUrlData(), agentId);

            return await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync<GetCountResult>(null, url, null, CommonJsonSendType.GET);
        }

        /// <summary>
        /// 【异步方法】获取素材列表
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="type"></param>
        /// <param name="agentId"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static async Task<BatchGetMaterialResult> BatchGetMaterialAsync(string accessToken, UploadMediaFileType type, int agentId, int offset, int count, int timeOut = Config.TIME_OUT)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/batchget?access_token={0}",
                accessToken.AsUrlData());

            var data = new
            {
                type = type.ToString(),
                agentid = agentId,
                offset = offset,
                count = count,
            };

            return await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync<BatchGetMaterialResult>(null, url, data, CommonJsonSendType.POST, timeOut);
        }

        /*上传的图片限制
          大小不超过2MB，支持JPG,PNG格式
          每天上传的图片不能超过100张*/
        /// <summary>
        /// 【异步方法】上传图文消息内的图片
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="media"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
       
        public static async Task<UploadimgMediaResult> UploadimgMediaAsync(string accessToken, string media, int timeOut = Config.TIME_OUT)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/uploadimg?access_token={0}",
                accessToken.AsUrlData());

            var data = new
            {
                media = media
            };

            return await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync<UploadimgMediaResult>(null, url, data, CommonJsonSendType.POST, timeOut);
        }
        #endregion
    }
}
