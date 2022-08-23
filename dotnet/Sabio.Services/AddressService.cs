using Sabio.Data.Providers;
using Sabio.Models.Domain.Addresses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data;
using Sabio.Models.Requests.Addresses;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class AddressService : IAddressService
    {
        private IDataProvider _data = null;
        public AddressService(IDataProvider data)
        {
            _data = data;
        }

        public int Add(AddressAddRequest aRequest, int userId)
        {
            int id = 0;
            string storedProc = "[dbo].[Sabio_Addresses_Insert]";
            _data.ExecuteNonQuery(storedProc, inputParamMapper: delegate (SqlParameterCollection requestCol)
            {
                AddCommonParams(aRequest, requestCol);

                //Get id output:
                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                requestCol.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection responseCol)
            {
                object oId = responseCol["@id"].Value;

                int.TryParse(oId.ToString(), out id);

            });
            return id;
        }

        public void Update(AddressUpdateRequest anUpdateRequest, int userId)
        {
            string storedProc = "[dbo].[Sabio_Addresses_Update]";
            _data.ExecuteNonQuery(storedProc, inputParamMapper: delegate (SqlParameterCollection requestCol)
            {
                AddCommonParams(anUpdateRequest, requestCol);
                requestCol.AddWithValue("@Id", anUpdateRequest.Id);

            }, returnParameters: null);

        }
        public Address Get(int id)
        {
            Address address = null;
            string procName = "[dbo].[Sabio_Addresses_SelectById]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                //Mapper for the input parameter
                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)
            {
                //Mapper for a single record read out of the DB
                //reader from DB >>> Adsress

                address = MapSingleAddress(reader);

            });

            return address;
        }

        public List<Address> GetRandomAddresses()
        {
            List<Address> addressList = null;
            string procName = "[dbo].[Sabio_Addresses_SelectRandom50]";

            _data.ExecuteCmd(procName, inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                //Mapper for a single record read out of the DB
                //reader from DB >>> Adsress

                Address address = MapSingleAddress(reader);

                if (addressList == null)
                {
                    addressList = new List<Address>();
                }

                addressList.Add(address);
            });
            return addressList;
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Sabio_Addresses_DeleteById]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                //Mapper for the input parameter
                paramCollection.AddWithValue("@Id", id);

            }, returnParameters: null);
        }
        private static Address MapSingleAddress(IDataReader reader)
        {
            Address address = new Address();
            int startingIndex = 0;

            address.Id = reader.GetSafeInt32(startingIndex++);
            address.LineOne = reader.GetSafeString(startingIndex++);
            address.SuiteNumber = reader.GetSafeInt32(startingIndex++);
            address.City = reader.GetSafeString(startingIndex++);
            address.State = reader.GetSafeString(startingIndex++);
            address.PostalCode = reader.GetSafeString(startingIndex++);
            address.IsActive = reader.GetSafeBool(startingIndex++);
            address.Lat = reader.GetSafeDouble(startingIndex++);
            address.Long = reader.GetSafeDouble(startingIndex++);
            return address;
        }

        private static void AddCommonParams(AddressAddRequest aRequest, SqlParameterCollection requestCol)
        {
            requestCol.AddWithValue("@LineOne", aRequest.LineOne);
            requestCol.AddWithValue("@SuiteNumber", aRequest.SuiteNumber);
            requestCol.AddWithValue("@City", aRequest.City);
            requestCol.AddWithValue("@State", aRequest.State);
            requestCol.AddWithValue("@PostalCode", aRequest.PostalCode);
            requestCol.AddWithValue("@IsActive", aRequest.IsActive);
            requestCol.AddWithValue("@Lat", aRequest.Lat);
            requestCol.AddWithValue("@Long", aRequest.Long);
        }
    }


}
