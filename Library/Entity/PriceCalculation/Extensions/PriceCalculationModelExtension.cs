using CalculationOilPrice.Library.Entity.PriceCalculation.Models;
using CalculationOilPrice.Library.Entity.Setting.PriceCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationOilPrice.Library.Entity.PriceCalculation.Extensions
{
    public static class PriceCalculationModelExtension
    {
        public static void SetBasicCalculationNote(this CalculationModel model)
        {
            //always start with zero
            CalculationNoteModel oItem = new CalculationNoteModel()
            {
                ID = 0,
                CalculationItems = new List<CalculationItemModel>(),
                //CalculationMarginItems = new List<CalculationItemModel>()
            };
            model.CalculationNotes.Add(oItem);


            //group1
            int order = 0;
            oItem.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "",
                Description = "Bareinkaufspreis",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "BEK",
                //Currency = generalSettingModel.Currency.Currency,
                Currency = new CurrencyModel() { Currency = "CHF" },
                CostCalculatonGroup = new CostCalculatonGroupModel()
                {
                    BaseCalculationGroupRows = new List<int>() { 3, 2, 1 },
                    SummaryGroups = new List<int> { 3 },
                },
                Group = 0,
                ItemOrder = order,
                Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            oItem.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Bezugskosten",
                Tag = "BZK",
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 1,
                ItemOrder = order,
                Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            if (model.GeneralSetting.TextLines != null)
            {
                int count = 0;
                foreach (string item in model.GeneralSetting.TextLines)
                {
                    count += 1;
                    order += 1;
                    oItem.CalculationItems.Add(new CalculationItemModel()
                    {
                        Sign = "+",
                        Description = item,
                        Tag = String.Format("BEN {0}", count),
                        Currency = new CurrencyModel() { Currency = "CHF" },
                        Group = 1,
                        ItemOrder = order,
                        Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                    });
                }
            }

            order += 1;
            oItem.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Einstandspreis",
                Tag = "ESTP",
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 1,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1 },
                CostCalculatonGroup = new CostCalculatonGroupModel()
                {
                    BaseCalculationGroupRows = new List<int>() { 3, 2 },
                    SummaryGroups = new List<int> { 3 },
                },
                ItemOrder = order,
                Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });


            //group2
            order += 1;
            oItem.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Verwaltungsgemeinkosten",
                Tag = "OGK",
                Currency = new CurrencyModel() { Currency = "CHF" },
                BaseCalculationGroupRows = new List<int>() { 0, 1 },
                Group = 2,
                ItemOrder = order,
                Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            oItem.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Vertriebsgemeinkosten",
                Tag = "VGK",
                Currency = new CurrencyModel() { Currency = "CHF" },
                BaseCalculationGroupRows = new List<int>() { 0, 1 },
                Group = 2,
                ItemOrder = order,
                Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            oItem.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Sondereinzelkosten des Vertriebs",
                Tag = "VSK",
                Currency = new CurrencyModel() { Currency = "CHF" },
                BaseCalculationGroupRows = new List<int>() { 0, 1 },
                Group = 2,
                ItemOrder = order,
                Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            oItem.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Verwaltungs- und Vertriebskosten",
                Tag = "VVK",
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 2,
                IsSummary = true,
                SummaryGroups = new List<int>() { 2 },
                CostCalculatonGroup = new CostCalculatonGroupModel()
                {
                    BaseCalculationGroupRows = new List<int>() { 2 },
                    SummaryGroups = new List<int>(),
                },
                ItemOrder = order,
                Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            oItem.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Einstandspreis",
                Tag = "ESTP",
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 2,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1 },
                CostCalculatonGroup = new CostCalculatonGroupModel()
                {
                    BaseCalculationGroupRows = new List<int>() { 3, 2 },
                    SummaryGroups = new List<int> { 3 },
                },
                ItemOrder = order,
                Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            oItem.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Selbstkosten 1",
                Tag = "SK 1",
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 2,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1, 2 },
                CostCalculatonGroup = new CostCalculatonGroupModel()
                {
                    BaseCalculationGroupRows = new List<int>() { 3 },
                    SummaryGroups = new List<int> { 3 },
                },
                ItemOrder = order,
                Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });


            //group3
            order += 1;
            oItem.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Lagerhaltungskosten",
                Tag = "LHK",
                Currency = new CurrencyModel() { Currency = "CHF" },
                BaseCalculationGroupRows = new List<int>() { 0, 1, 2 },
                Group = 3,
                ItemOrder = order,
                Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            oItem.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Verpackungsanteil",
                Tag = "VPA",
                Currency = new CurrencyModel() { Currency = "CHF" },
                BaseCalculationGroupRows = new List<int>() { 0, 1, 2 },
                Group = 3,
                ItemOrder = order,
                Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            oItem.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Transportanteil",
                Tag = "TRA",
                Currency = new CurrencyModel() { Currency = "CHF" },
                BaseCalculationGroupRows = new List<int>() { 0, 1, 2 },
                Group = 3,
                ItemOrder = order,
                Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            oItem.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Selbstkosten 2",
                Tag = "SK 2",
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 3,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1, 2, 3 },
                CostCalculatonGroup = new CostCalculatonGroupModel()
                {
                    BaseCalculationGroupRows = new List<int>() { 4 },
                    SummaryGroups = new List<int> { 4 },
                },
                ItemOrder = order,
                Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });
        }

        public static void SetScaleCalculationNote(this CalculationModel model, PriceSetting priceSetting, int starItemtOrderID)
        {
            //price scale id start by 1
            for (int i = 1; i <= model.GeneralSetting.PriceScale.Scale; i++)
            {
                int iOrder = starItemtOrderID;

                CalculationNoteModel oItem = new CalculationNoteModel()
                {
                    ID = i,
                    CalculationItems = new List<CalculationItemModel>(),
                    //CalculationMarginItems = new List<CalculationItemModel>()
                };
                model.CalculationNotes.Add(oItem);

                //group4   
                iOrder += 1;
                oItem.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Gewinnaufschlag",
                    Tag = "GA",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    BaseCalculationGroupRows = new List<int>() { 0, 1, 2, 3 },
                    Group = 4,
                    ItemOrder = iOrder,
                    Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });

                iOrder += 1;
                oItem.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "=",
                    Description = "Barverkaufspreis",
                    Tag = "VK(bar)",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 4,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1, 2, 3, 4 },
                    CostCalculatonGroup = new CostCalculatonGroupModel()
                    {
                        BaseCalculationGroupRows = new List<int>() { 5 },
                        SummaryGroups = new List<int> { 5 },
                    },
                    ItemOrder = iOrder,
                    Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });


                //group5
                iOrder += 1;
                oItem.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Kundenskonto",
                    AmountPercent = priceSetting.CashDiscount,
                    Tag = "SKT",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    BaseCalculationGroupRows = new List<int>() { 0, 1, 2, 3, 4 },
                    Group = 5,
                    ItemOrder = iOrder,
                    Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null,
                    EditedField = "P"
                });

                iOrder += 1;
                oItem.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Verhandlungsspielraum",
                    AmountPercent = priceSetting.SalesBonus,
                    Tag = "PV",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    BaseCalculationGroupRows = new List<int>() { 0, 1, 2, 3, 4 },
                    Group = 5,
                    ItemOrder = iOrder,
                    Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null,
                    EditedField = "P"
                });

                iOrder += 1;
                oItem.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "=",
                    Description = "Zielverkaufspreis",
                    Tag = "VK(ziel)",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 5,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1, 2, 3, 4, 5 },
                    CostCalculatonGroup = new CostCalculatonGroupModel()
                    {
                        BaseCalculationGroupRows = new List<int>() { 6 },
                        SummaryGroups = new List<int> { 6 },
                    },
                    ItemOrder = iOrder,
                    Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });


                //group6
                iOrder += 1;
                oItem.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Kundenrabatt",
                    AmountPercent = priceSetting.CustomerDiscount,
                    Tag = "RBT",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    BaseCalculationGroupRows = new List<int>() { 0, 1, 2, 3, 4, 5 },
                    Group = 6,
                    ItemOrder = iOrder,
                    Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null,
                    EditedField = "P"
                });

                iOrder += 1;
                oItem.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "=",
                    Description = "Nettoverkaufspreis",
                    Tag = "VK(liste)",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 6,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                    CostCalculatonGroup = new CostCalculatonGroupModel()
                    {
                        BaseCalculationGroupRows = new List<int>() { 7 },
                        SummaryGroups = new List<int> { 7 },
                    },
                    ItemOrder = iOrder,
                    Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });


                //group7
                iOrder += 1;
                oItem.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Mehrwertsteuer",
                    AmountPercent = priceSetting.VatTaxes,
                    Tag = "MWST",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    BaseCalculationGroupRows = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                    Group = 7,
                    ItemOrder = iOrder,
                    Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null,
                    EditedField = "P"
                });

                iOrder += 1;
                oItem.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "=",
                    Description = "Bruttoverkaufspreis",
                    Tag = "VK(brutto)",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 7,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 },
                    ItemOrder = iOrder,
                    Convert = model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });
            }
        }

        public static void SetMarginCalculationNote(this CalculationModel model)
        {
            //add margin items for each note
            foreach (CalculationNoteModel item in model.CalculationNotes)
            {
                int order = 0;
                item.CalculationMarginItems = new List<CalculationItemModel>();

                //master amount
                //int order = 0;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "",
                    Description = "Bareinkaufspreis",
                    Tag = "BEK",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 0,
                    ItemOrder = order
                });


                //group1
                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Bezugskosten",
                    Tag = "BZK",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 1,
                    ItemOrder = order
                });

                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Beschaffungsteilkosten",
                    Tag = "BEN",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 1,
                    ItemOrder = order
                });

                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "",
                    Description = "Einstandspreis",
                    Tag = "ESTP",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 1,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1 },
                    ItemOrder = order
                });


                //group2
                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Verwaltungsgemeinkosten",
                    Tag = "OGK",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    //CalculationBaseGroupRows = new List<int>() { 0, 1 },
                    Group = 2,
                    ItemOrder = order
                });

                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Vertriebsgemeinkosten",
                    Tag = "VGK",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    //CalculationBaseGroupRows = new List<int>() { 0, 1 },
                    Group = 2,
                    ItemOrder = order
                });

                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Sondereinzelkosten des Vertriebs",
                    Tag = "VSK",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    //CalculationBaseGroupRows = new List<int>() { 0, 1 },
                    Group = 2,
                    ItemOrder = order
                });

                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "",
                    Description = "Verwaltungs- und Vertriebskosten",
                    Tag = "VVK",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 2,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 2 },
                    ItemOrder = order
                });


                //group3
                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "",
                    Description = "Summe fix/variabel",
                    Tag = "SK 1",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 3,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1, 2 },
                    ItemOrder = order
                });


                //group4                  
                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "",
                    Description = "Gewinnaufschlag",
                    Tag = "GA",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    //IsSummary = true,
                    //CalculationBaseGroupRows = new List<int>() { 0, 1, 2 },
                    //SummaryGroups = new List<int>() { 0, 1, 2, 3 },
                    Group = 4,
                    ItemOrder = order
                });


                //group5
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "",
                    Description = "Deckungsbeitrag",
                    Tag = "VK",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 5,
                    //CalculationBaseGroupRows = new List<int>() { 0, 1, 2, 3 },
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1, 2, 4 },
                    ItemOrder = order
                });


                //group6
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "",
                    Description = "Barverkaufspreis",
                    Tag = "VK(bar)",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 6,
                    //IsSummary = true,
                    //SummaryGroups = new List<int>() { 0, 1, 2, 3, 4, 5 },
                    ItemOrder = order
                });


                //group7
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "",
                    Description = "Bruttoverkaufspreis",
                    Tag = "VK(brutto)",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 7,
                    //IsSummary = true,
                    //SummaryGroups = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                    ItemOrder = order
                });

            }
        }

        public static void SetCalculationView(this CalculationModel model)
        {
            //add view items
            model.CalculationViewItems = new List<CalculationItemModel>();

            try
            {
                model.CalculationViewItems.AddRange(model.CalculationNotes[0].CalculationItems);
            }
            catch { }
        }

        public static void SetCalculationMarginView(this CalculationModel model)
        {
            //add view items
            model.CalculationMarginViewItems = new List<CalculationItemModel>();

            try
            {
                model.CalculationMarginViewItems.AddRange(model.CalculationNotes[0].CalculationMarginItems);
            }
            catch { }
        }
    }
}
