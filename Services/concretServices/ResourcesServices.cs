using App_FDark.Models;
using App_FDark.Services.abstractServices;
using System.Collections.Generic;

namespace App_FDark.Services.concretServices
{
    public class ResourcesServices : IResourcesServices
    {
        public ResourcesServices() { }
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
                ResourceVideo video = new ResourceVideo();
                video.Label = l.Label;
                video.Url = l.Url;
                video.Description = l.Description;
                int startIndex = l.Url.LastIndexOf("v=") + 2;
                int endIndex = l.Url.Length-startIndex;
                if (l.Url.Substring(startIndex,endIndex).Contains("&"))
                {
                    endIndex = l.Url.LastIndexOf("&") - startIndex;
                }
                video.VideoId = l.Url.Substring(startIndex,endIndex);
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
                ResourceSite site = new ResourceSite();
                site.Label = l.Label;
                site.Url = l.Url;
                site.Picture = l.Picture;
                site.Description = l.Description;
                siteListVm.Add(site);
            }
            return siteListVm;
        }

        public List<ResourceImage> CreateImageList(List<Links> linksList)
        {
            List<ResourceImage> imageListVm = new List<ResourceImage>();
            List<Links> imageList = linksList.Where(l => l.DataType == "image").ToList();
            foreach (var l in imageList)
            {
                ResourceImage image = new ResourceImage();
                image.Label = l.Label;
                image.Description = l.Description;
                if (!String.IsNullOrEmpty(l.Picture))
                {
                    image.Pictures = l.Picture.Split(',');
                }
                else
                {
                    string[] emptyImages = { "" };
                    image.Pictures = emptyImages;
                }
                imageListVm.Add(image);
            }
            return imageListVm;
        }
    }
}
