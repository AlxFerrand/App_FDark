using App_FDark.Data;
using App_FDark.Models;
using App_FDark.Services.abstractServices;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Policy;

namespace App_FDark.Services.concretServices
{
    public class ResourcesServices : IResourcesServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ISaveFilesService _saveFilesService;
        public ResourcesServices(ApplicationDbContext context, ISaveFilesService saveFilesService) 
        {
            _context = context;
            _saveFilesService = saveFilesService;
        }
        public CatalogViewModel CreateCatalogViewModel(List<Links> linksList)
        {
            CatalogViewModel vm = new CatalogViewModel();
            vm.Videos = CreateVideoList(linksList);
            vm.Sites = CreateSiteList(linksList);
            vm.Images = CreateImageList(linksList);
            return vm;
        }

        public List<ResourceVideo> CreateVideoList(List<Links> linksList)
        {
            List<ResourceVideo> videoListVm = new List<ResourceVideo>();
            List<Links> videoLinksList = linksList.Where(l => l.DataType == "video").ToList();
            foreach (var l in videoLinksList)
            {
                ResourceVideo video = CreateVideoResource(l.Label, l.Url, l.Description);
                videoListVm.Add(video);
            }
            return videoListVm;
        }

        public List<ResourceSite> CreateSiteList(List<Links> linksList)
        {
            List<ResourceSite> siteListVm = new List<ResourceSite>();
            List<Links> siteList = linksList.Where(l => l.DataType == "site").ToList();
            foreach (var l in siteList)
            {
                ResourceSite site = CreateSiteResource(l.Label,l.Url,l.Description,l.Picture);
                siteListVm.Add(site);
            }
            return siteListVm;
        }

        public List<ResourceImage> CreateImageList(List<Links> linksList)
        {
            List<ResourceImage> imageListVm = new List<ResourceImage>();
            List<Links> imageList = linksList.Where(l => l.DataType == "img").ToList();
            foreach (var l in imageList)
            {
                ResourceImage image = CreateImageResource(l.Label,l.Description,l.Picture);
                imageListVm.Add(image);
            }
            return imageListVm;
        }
        public ResourceVideo CreateVideoResource(string label, string url, string description)
        {
            ResourceVideo video = new ResourceVideo();
            video.Label = label;
            video.Url = url;
            video.Description = description;

            if ((!String.IsNullOrEmpty(url)) && url.Contains("youtube.com"))
            {
                var uri = new Uri(url);
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                video.VideoId = query["v"];
            }
            else
            {
                video.VideoId = url;
            }
            return video;
        }
        public ResourceSite CreateSiteResource(string label, string url, string description, string pictures)
        {
            ResourceSite site = new ResourceSite();
            site.Label = label;
            site.Url = url;
            site.Description = description;
            site.Picture = pictures.Split(",")[0];
            return site;
        }
        public ResourceImage CreateImageResource(string label, string description, string picture)
        {
            ResourceImage image = new ResourceImage();
            image.Label = label;
            image.Description = description;
            if (!String.IsNullOrEmpty(picture))
            {
                image.Pictures = picture.Split(',');
            }
            else
            {
                string[] emptyImages = { "" };
                image.Pictures = emptyImages;
            }
            return image;
        }

        public List<ResourceAdminViewModel> CreateResourceAdminViewModel(string order,string dataTypeSort,int statusId,int extId,int contentId)
        {
            List<Links> linksList = new List<Links>();
            if (contentId != 0)
            {
                linksList = _context.Links.Where(l => l.ContentId == contentId).ToList();
            }
            else
            {
                if (extId != 0)
                {
                    List<Content> contentList = _context.Content.Where(c=>c.ExtensionId == extId).ToList();
                    foreach(Content content in contentList)
                    {
                        linksList.AddRange(_context.Links.Where(l=>l.ContentId == content.Id).ToList());
                    }
                }
            }
            if(linksList.Count == 0)
            {
                if (!String.IsNullOrEmpty(dataTypeSort))
                {
                    linksList = _context.Links.Where(l => l.DataType.Equals(dataTypeSort)).ToList();
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(dataTypeSort))
                {
                    linksList = linksList.Where(l => l.DataType.Equals(dataTypeSort)).ToList();
                }
            }
            if (linksList.Count == 0)
            {
                if (statusId != 0)
                {
                    linksList = _context.Links.Where(l => l.Status == statusId).ToList();
                }
            }
            else
            {
                if (statusId != 0)
                {
                    linksList = linksList.Where(l => l.Status == statusId).ToList();
                }
            }

            if (String.IsNullOrEmpty(dataTypeSort) && statusId==0 && extId==0 && contentId==0)
            {
                linksList = _context.Links.ToList();
            }


                List<ResourceAdminViewModel> resourcesList = new List<ResourceAdminViewModel>();
            foreach (var l in linksList)
            {
                int id = l.Id;
                string label = TruncateString(l.Label,25);
                string picture = TruncateString(l.Picture,30);
                string url = TruncateString(l.Url,40);
                string description = TruncateString(l.Description,40);
                string content = _context.Content.Where(c => c.Id == l.ContentId).FirstOrDefault().Name;
                string status = StatusDictionary.statusDictionary[l.Status];
                string dataType = l.DataType;

                resourcesList.Add(new ResourceAdminViewModel(id, label, picture, url, description, content, status, dataType));
            }
            resourcesList = OrderResourcesList(resourcesList, order);
            return resourcesList;
        }

        public string TruncateString (string stringToTruncate, int length)
        {
            if (!(String.IsNullOrEmpty(stringToTruncate)) &&(stringToTruncate.Length > length))
            {
                stringToTruncate = stringToTruncate.Substring(0, length) + "...";
            }
            return stringToTruncate;
        }
        public List<ResourceAdminViewModel> OrderResourcesList(List<ResourceAdminViewModel> resourcesList, string order)
        {
            switch (order)
            {
                case "label":
                    resourcesList = resourcesList.OrderBy(r => r.Label).ToList();
                    break;
                case "content":
                    resourcesList = resourcesList.OrderBy(r => r.Content).ToList();
                    break;
                case "status":
                    resourcesList = resourcesList.OrderBy(r => r.Status).ToList();
                    break;
                case "dataType":
                    resourcesList = resourcesList.OrderBy(r => r.DataType).ToList();
                    break;
                case "label_desc":
                    resourcesList = resourcesList.OrderByDescending(r => r.Label).ToList();
                    break;
                case "content_desc":
                    resourcesList = resourcesList.OrderByDescending(r => r.Content).ToList();
                    break;
                case "status_desc":
                    resourcesList = resourcesList.OrderByDescending(r => r.Status).ToList();
                    break;
                case "dataType_desc":
                    resourcesList = resourcesList.OrderByDescending(r => r.DataType).ToList();
                    break;
                default:
                    resourcesList = resourcesList.OrderBy(r => r.Id).ToList();
                    break;
            }
            return resourcesList;
        }

        public Links CreateNewRessource(string dataType, string Label, string Url, string Description, int contentId, List<IFormFile> files)
        {
            Links newLink = new Links();
            newLink.Label = Label;
            newLink.Description = Description;
            newLink.ContentId = contentId;
            newLink.DataType = dataType;
            newLink.Status = 1;

            if (dataType.Equals("video") || dataType.Equals("site"))
            {
                newLink.Url = Url;
            }
            if (dataType.Equals("site"))
            {
                newLink.Picture = FilesNameConstructor(files[0],contentId.ToString(),false);
            }
            if (dataType.Equals("img"))
            {
                newLink.Picture = FilesNameConstructor(files, contentId.ToString(),false);
            }
            return newLink;
            
        }
        public Links EditResource(Links link, List<IFormFile> files, string oldPicture)
        {
            if(files.Count > 0)
            {
                if (link.DataType.Equals("site"))
                {
                    _saveFilesService.CleanStringsFiles(oldPicture);
                    link.Picture = FilesNameConstructor(files[0], link.ContentId.ToString(), false);
                }
                if (link.DataType.Equals("img"))
                {
                    _saveFilesService.CleanStringsFiles(oldPicture);
                    link.Picture = FilesNameConstructor(files, link.ContentId.ToString(), false);
                }
            }
            return link;
        }
        public void DeleteFileResource(int id)
        {
            string pictures = _context.Links.Find(id).Picture;
            _saveFilesService.CleanStringsFiles(pictures);
        }

        public string FilesNameConstructor(List<IFormFile> files, string prefix, bool temp)
        {
            int index = 1;
            string filesName = "";
            foreach (IFormFile file in files)
            {
                if (!String.IsNullOrEmpty(filesName))
                {
                    if (temp)
                    {
                        filesName = filesName + "," + _saveFilesService.SaveFileToImgDirectory(file,"TEMP_" + prefix + "_" + index + "_");
                    }
                    else
                    {
                        filesName = filesName + "," + _saveFilesService.SaveFileToImgDirectory(file, prefix + "_" + index + "_");
                    }
                    
                }
                else
                {
                    if (temp)
                    {
                        filesName = _saveFilesService.SaveFileToImgDirectory(file, "TEMP_" + prefix + "_" + index + "_");
                    }
                    else
                    {
                        filesName = _saveFilesService.SaveFileToImgDirectory(file, prefix + "_" + index + "_");
                    }
                    
                }
                index++;
            }
            return filesName;
        }
        public string FilesNameConstructor(IFormFile file, string prefix, bool temp)
        {
            string fileName = "";
            if (temp)
            {
                fileName = _saveFilesService.SaveFileToImgDirectory(file, "TEMP_" + prefix + "_");
            }
            else
            {
                fileName = _saveFilesService.SaveFileToImgDirectory(file, prefix + "_");
            }
            return fileName;
        }

        public Object MakeSnapResource(int id, string dataType, string label, string url, string description, string pictures)
        {
            switch (dataType)
            {
                case "video":
                    ResourceVideo video = CreateVideoResource(label, url, description);
                    return video;
                    break;
                case "site":
                    ResourceSite site = CreateSiteResource(label, url, description, pictures);   
                    return site;
                    break;
                case "img":                  
                    ResourceImage image = CreateImageResource(label,description,pictures);
                    return image;
                    break;
                case "text":
                    return null;
                    break;
                default:
                    return null;
                    break;
            }
        }

        public int[] NewsCounter()
        {
            int[] newsCounter = new int[2];
            newsCounter[0] = _context.Links.Where(l=>l.Status==1).ToList().Count();
            newsCounter[1] = _context.Users.Where(u => u.EmailConfirmed == false).ToList().Count();
            return newsCounter;
        }
    }
}
