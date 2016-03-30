//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RoboUtil.managers
//{
//    public class SisLocalizationManager 
//    {
//        private SisLocalizationDao _sisLocalizationDao;

//        public SisLocalizationManager()
//        {
//            Logger.Error("!! UYARI Manager Class larini new ile olusturmayiniz UsisService.ManagerName seklinde kullaniniz !!");
//        }

//        public WcfMessage GetAllSisLocalization()
//        {
//            WcfMessage message = new WcfMessage();
//            List<SisLocalizationDto> sonuc = null;
//            try
//            {
//                _sisLocalizationDao = new SisLocalizationDao(UsisManager.Database.Context, UsisManager.UserSession);

//                sonuc = _sisLocalizationDao.GetAllSisLocalization();

//                message.Data.Add("sislocalizations", sonuc);
//            }
//            catch (UsisException ex)
//            {
//                Logger.Error(ex.Message);
//                throw;
//            }
//            catch (Exception ex)
//            {
//                Logger.Error("hata:", ex);
//                throw;
//            }
//            return message;
//        }

//        public WcfMessage InsertSisLocalization(SisLocalizationDto localizationDto)
//        {
//            WcfMessage message = new WcfMessage();
//            try
//            {
//                _sisLocalizationDao = new SisLocalizationDao(UsisManager.Database.Context, UsisManager.UserSession);

//                SisLocalizationDto pt = _sisLocalizationDao.Insert(localizationDto.MapTo<SisLocalization>()).MapTo<SisLocalizationDto>();

//                UsisManager.SisCacheInfoUpdate(UsisConstants.LOCALIZATION_CACHE);

//                UsisManager.Database.Commit();

//                message.Data.Add("sislocalizations", pt);
//            }
//            catch (UsisException ex)
//            {
//                Logger.Error(ex.Message);
//                throw;
//            }
//            catch (Exception ex)
//            {
//                Logger.Error("hata:", ex);
//                throw;
//            }
//            return message;
//        }

//        public WcfMessage UpdateSisLocalization(SisLocalizationDto localizationDto)
//        {
//            WcfMessage message = new WcfMessage();
//            try
//            {
//                _sisLocalizationDao = new SisLocalizationDao(UsisManager.Database.Context, UsisManager.UserSession);

//                SisLocalizationDto pt = _sisLocalizationDao.Update(localizationDto.MapTo<SisLocalization>()).MapTo<SisLocalizationDto>();

//                UsisManager.SisCacheInfoUpdate(UsisConstants.LOCALIZATION_CACHE);

//                UsisManager.Database.Commit();

//                message.Data.Add("sislocalizations", pt);
//            }
//            catch (UsisException ex)
//            {
//                Logger.Error(ex.Message);
//                throw;
//            }
//            catch (Exception ex)
//            {
//                Logger.Error("hata:", ex);
//                throw;
//            }
//            return message;
//        }

//        public WcfMessage DeleteSisLocalization(int pk)
//        {
//            WcfMessage message = new WcfMessage();
//            try
//            {
//                _sisLocalizationDao = new SisLocalizationDao(UsisManager.Database.Context, UsisManager.UserSession);

//                var pt = _sisLocalizationDao.Delete(new SisLocalization() { Pk = pk });

//                UsisManager.SisCacheInfoUpdate(UsisConstants.LOCALIZATION_CACHE);

//                UsisManager.Database.Commit();

//                message.Data.Add("sislocalizations", pt);
//            }
//            catch (UsisException ex)
//            {
//                Logger.Error(ex.Message);
//                throw;
//            }
//            catch (Exception ex)
//            {
//                Logger.Error("hata:", ex);
//                throw;
//            }
//            return message;
//        }

//        public WcfMessage GetLinqDynamicLocalization(string where, string orderby, int skip, int take, params object[] values)
//        {
//            WcfMessage message = new WcfMessage();
//            List<SisLocalizationDto> sonuc = null;
//            try
//            {
//                _sisLocalizationDao = new SisLocalizationDao(UsisManager.Database.Context, UsisManager.UserSession);
//                sonuc = _sisLocalizationDao.GetLinqDynamic(where, orderby, skip, take, values).MapTo<List<SisLocalizationDto>>();
//                message.Data.Add("sislocalizations", sonuc);
//            }
//            catch (UsisException ex)
//            {
//                Logger.Error(ex.Message);
//                throw;
//            }
//            catch (Exception ex)
//            {
//                Logger.Error("hata:", ex);
//                throw;
//            }
//            return message;
//        }

//        public WcfMessage SaveOrUpdateSisLocalization(SisLocalizationDto localizationDto)
//        {
//            WcfMessage message = new WcfMessage();
//            try
//            {
//                _sisLocalizationDao = new SisLocalizationDao(UsisManager.Database.Context, UsisManager.UserSession);

//                SisLocalizationDto pt = _sisLocalizationDao.SaveOrUpdate(localizationDto.MapTo<SisLocalization>()).MapTo<SisLocalizationDto>();

//                UsisManager.SisCacheInfoUpdate(UsisConstants.LOCALIZATION_CACHE);

//                UsisManager.Database.Commit();

//                message.Data.Add("sislocalizations", pt);
//            }
//            catch (UsisException ex)
//            {
//                Logger.Error(ex.Message);
//                throw;
//            }
//            catch (Exception ex)
//            {
//                Logger.Error("hata:", ex);
//                throw;
//            }
//            return message;
//        }


//    }
//}
