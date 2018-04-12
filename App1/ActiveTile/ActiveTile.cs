using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications; // Notifications library
using Windows.Data.Xml.Dom;
using System.Xml.Linq;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.AccessCache;
using Windows.Storage.Streams;

namespace App1.ActiveTile
{
    class ActiveTile
    {

        /*public static XmlDocument CreateTiles(Models.ListItem item)
        {
            XDocument xDoc = new XDocument(
                new XElement("tile", new XAttribute("version", 3),
                    new XElement("visual",
                        // Small Tile  
                        new XElement("binding", new XAttribute("branding", "none"),
                            new XAttribute("displayName", "qqq"), new XAttribute("template", "TileSmall")
                            ),

                        // Medium Tile  
                        new XElement("binding", new XAttribute("branding", "name"),
                            new XAttribute("displayName", "www"), new XAttribute("template", "TileMedium"),
                            new XElement("group",
                                new XElement("subgroup",
                                    new XElement("text", item.Plan_date, new XAttribute("hint-style", "caption")),
                                    new XElement("text", item.Title,
                                        new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true),
                                        new XAttribute("hint-maxLines", 3))
                                    )
                                )
                            ),

                        // Wide Tile  
                        new XElement("binding", new XAttribute("branding", "name"),
                            new XAttribute("displayName", "eee"), new XAttribute("template", "TileWide"),
                            new XElement("group",
                                new XElement("subgroup",
                                    new XElement("text", item.Plan_date, new XAttribute("hint-style", "caption")),
                                    new XElement("text", item.Title,
                                        new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true),
                                        new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", item.Content,
                                        new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true),
                                        new XAttribute("hint-maxLines", 3))
                                    ),
                                new XElement("subgroup", new XAttribute("hint-weight", 15),
                                    new XElement("image", new XAttribute("placement", "inline"),
                                        new XAttribute("src", "Assets/StoreLogo.png"))
                                    )
                                )
                            ),

                        //Large Tile  
                        new XElement("binding", new XAttribute("branding", "title"),
                            new XAttribute("displayName", "rrr"), new XAttribute("template", "TileLarge"),
                            new XElement("group",
                                new XElement("subgroup",
                                    new XElement("text", item.Plan_date, new XAttribute("hint-style", "caption")),
                                    new XElement("text", item.Title,
                                        new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true),
                                        new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", item.Content,
                                        new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true),
                                        new XAttribute("hint-maxLines", 3))
                                    ),
                                new XElement("subgroup", new XAttribute("hint-weight", 15),
                                    new XElement("image", new XAttribute("placement", "inline"),
                                        new XAttribute("src", "Assets/StoreLogo.png"))
                                    )
                                )
                            )
                        )
                    )
                );

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xDoc.ToString());
            //Debug.WriteLine(xDoc);  
            return xmlDoc;
        }*/

        public static void Update(Models.ListItem item)
        {
            // In a real app, these would be initialized with actual data
            string time = item.Plan_date.ToString();
            string subject = item.Title;
            string body = item.Content;


            // Construct the tile content
            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileSmall = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            BackgroundImage = new TileBackgroundImage()
                            {
                                Source = "Assets/StoreLogo.png"
                            },

                            Children =
                            {
                                
                            }
                        }
                    },

                    TileMedium = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            BackgroundImage = new TileBackgroundImage()
                            {
                                Source = item.ImagePath
                            },

                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = subject
                                },

                                new AdaptiveText()
                                {
                                    Text = body,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle
                                },

                                new AdaptiveText()
                                {
                                    Text = time,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle
                                }
                            }
                        }
                    },

                    TileWide = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            BackgroundImage = new TileBackgroundImage()
                            {
                                 Source = item.ImagePath
                            },

                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = subject
                                },

                                new AdaptiveText()
                                {
                                    Text = body,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle
                                },

                                new AdaptiveText()
                                {
                                    Text = time,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle
                                }
                            }
                        }
                    }
                }
            };


            // Then create the tile notification
            var notification = new TileNotification(content.GetXml());


            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            // And send the notification
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }

        public static void UpdateForAll()
        {
            ViewModels.ListItemViewModels ViewModel = ViewModels.ListItemViewModels.getListItemViewModels();
            Clear();
            foreach (var item in ViewModel.AllItems)
            {
                Update(item);
            }
        }

        public static void Clear()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
        }
    }
}
