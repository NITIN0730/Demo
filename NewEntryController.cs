using Microsoft.AspNetCore.Mvc;
using System.Data;
using Npgsql;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using ApiCalling.Model;
using System.Reflection;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.IO;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ApiCalling.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewEntryController : ControllerBase
    {
        private readonly string _connectionString;

        public NewEntryController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("NewEntryPost")]
        public IActionResult NewEntryPost(NewEntryModel model)
        {
            try
            {
                ReturnResponse returnResponse = new ReturnResponse();
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    
                    var dp = new Dapper.DynamicParameters();
                    dp.Add("inentry_no", value: model.EntryNo, dbType: DbType.String);
                    dp.Add("inentry_date", value: model.EntryDate, dbType: DbType.String);
                    dp.Add("incustomer_name", value: model.CustomerName, dbType: DbType.String);
                    dp.Add("eninvoice_no", value: model.InvoiceNo, dbType: DbType.String);
                    dp.Add("eninvoice_date", value: model.InvoiceDate, dbType: DbType.String);

                    dp.Add("eninternal_note", value: model.InternalNote, dbType: DbType.String);
                    dp.Add("indsn", value: model.DSN, dbType: DbType.String);
                    dp.Add("inmodel_name", value: model.Model_Name, dbType: DbType.String);
                    dp.Add("inimei", value: model.IMEI, dbType: DbType.String);
                    dp.Add("intotal_package", value: model.TotalPackage, dbType: DbType.Int32);
                    dp.Add("intotal_item_count", value: model.TotalItemCount, dbType: DbType.Int32);

                    dp.Add("rspcode", dbType: DbType.String, direction: ParameterDirection.Output);
                    dp.Add("rspmsg", dbType: DbType.String, direction: ParameterDirection.Output);


                    connection.Open();
                    string ret = connection.Query<string>("insert_entry", dp, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    //string rspjson = dp.Get<string>("rspdata");
                    returnResponse.ResponseCode = dp.Get<string>("rspcode");
                    returnResponse.ResponseMessage = dp.Get<string>("rspmsg");
                }

                return Ok(returnResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("PackageApi")]
        public IActionResult PackageApi([FromBody] PACKAGEAPI_MODEL jobject)
        {
            List<ModelDataBind> modelDataBinds = new List<ModelDataBind>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    var dp = new Dapper.DynamicParameters();
                    dp.Add("entry_no_param", value: jobject.EntryNo, dbType: DbType.String);
                    modelDataBinds = connection.Query<ModelDataBind>("PACKAGES", dp, commandType: CommandType.StoredProcedure).ToList();
                }
                catch(Exception ex) {
                    return StatusCode(500, "An error occurred while processing the request");
                }
               

            }
            
                return Ok(modelDataBinds);
        }

        [HttpPost("PackageShowApi")]
        public IActionResult PackageShowApi([FromBody] PACKAGEAPI_MODEL jobject)
        {
            List<ModelDataBindAPI> modelDataBinds = new List<ModelDataBindAPI>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    var dp = new Dapper.DynamicParameters();
                    dp.Add("entry_no_param", value: jobject.EntryNo, dbType: DbType.String);
                    modelDataBinds = connection.Query<ModelDataBindAPI>("packagesAPI", dp, commandType: CommandType.StoredProcedure).ToList();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "An error occurred while processing the request");
                }


            }

            return Ok(modelDataBinds);
        }

        [HttpPost("PackageShowDSN")]
        public IActionResult PackageShowDSN([FromBody] PACKAGEAPI_MODEL jobject)
        {
            List<NewEntryDSNApi> modelDataBinds = new List<NewEntryDSNApi>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    var dp = new Dapper.DynamicParameters();
                    dp.Add("entry_no_param", value: jobject.EntryNo, dbType: DbType.String);
                    modelDataBinds = connection.Query<NewEntryDSNApi>("ShowData", dp, commandType: CommandType.StoredProcedure).ToList();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "An error occurred while processing the request");
                }


            }

            return Ok(modelDataBinds);
        }

        [HttpPost("ShowDSN")]
        public IActionResult ShowDSN([FromBody] DSNAPI_MODEL jobject)
        {
            List<ShowDSN> modelDataBinds = new List<ShowDSN>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    var dp = new Dapper.DynamicParameters();
                    dp.Add("entry_no_param", value: jobject.EntryNo, dbType: DbType.String);
                    dp.Add("varcarton", value: jobject.carton_label, dbType: DbType.String);
                    modelDataBinds = connection.Query<ShowDSN>("showdsn", dp, commandType: CommandType.StoredProcedure).ToList();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "An error occurred while processing the request");
                }


            }

            return Ok(modelDataBinds);
        }


        [HttpPost("NewEntry")]
        public IActionResult NewEntry(NewEntry model)
        {
            try
            {
                ReturnResponse returnResponse = new ReturnResponse();
                using (var connection = new NpgsqlConnection(_connectionString))
                {

                    var dp = new Dapper.DynamicParameters();
                    dp.Add("inentry_no", value: model.EntryNo, dbType: DbType.String);
                    dp.Add("inentry_date", value: model.EntryDate, dbType: DbType.String);
                    dp.Add("incustomer_name", value: model.CustomerName, dbType: DbType.String);
                    dp.Add("eninvoice_no", value: model.InvoiceNo, dbType: DbType.String);
                    dp.Add("eninvoice_date", value: model.InvoiceDate, dbType: DbType.String);

                    dp.Add("eninternal_note", value: model.InternalNote, dbType: DbType.String);
                    /*dp.Add("indsn", value: model.DSN, dbType: DbType.String);
                    dp.Add("inmodel_name", value: model.Model_Name, dbType: DbType.String);
                    dp.Add("inimei", value: model.IMEI, dbType: DbType.String);*/
                    dp.Add("intotal_package", value: model.TotalPackage, dbType: DbType.Int32);
                    dp.Add("intotal_item_count", value: model.TotalItemCount, dbType: DbType.Int32);

                    dp.Add("rspcode", dbType: DbType.String, direction: ParameterDirection.Output);
                    dp.Add("rspmsg", dbType: DbType.String, direction: ParameterDirection.Output);


                    connection.Open();
                    string ret = connection.Query<string>("insert_NewEntry", dp, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    //string rspjson = dp.Get<string>("rspdata");
                    returnResponse.ResponseCode = dp.Get<string>("rspcode");
                    returnResponse.ResponseMessage = dp.Get<string>("rspmsg");
                }

                return Ok(returnResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("NewEntryDSN")]
        public IActionResult NewEntryDSN(NewEntryDSN model)
        {
            try
            {
                ReturnResponse returnResponse = new ReturnResponse();
                using (var connection = new NpgsqlConnection(_connectionString))
                {

                    var dp = new Dapper.DynamicParameters();
                    dp.Add("indsn", value: model.DSN, dbType: DbType.String);
                    dp.Add("inmodel_name", value: model.Model_Name, dbType: DbType.String);
                    dp.Add("inimei", value: model.IMEI, dbType: DbType.String);
                    dp.Add("incarton_label", value: model.carton_label, dbType: DbType.String);
                    dp.Add("inentry_no", value: model.EntryNo, dbType: DbType.String);

                    dp.Add("rspcode", dbType: DbType.String, direction: ParameterDirection.Output);
                    dp.Add("rspmsg", dbType: DbType.String, direction: ParameterDirection.Output);


                    connection.Open();
                    string ret = connection.Query<string>("insert_DSN", dp, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    //string rspjson = dp.Get<string>("rspdata");
                    returnResponse.ResponseCode = dp.Get<string>("rspcode");
                    returnResponse.ResponseMessage = dp.Get<string>("rspmsg");
                }

                return Ok(returnResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost("Newpackages")]
        public IActionResult Newpackages(NewPackages model)
        {
            try
            {
                ReturnResponse returnResponse = new ReturnResponse();
                using (var connection = new NpgsqlConnection(_connectionString))
                {

                    var dp = new Dapper.DynamicParameters();
                    dp.Add("indsn", value: model.DSN, dbType: DbType.String);
                    dp.Add("inmodel_name", value: model.Model_Name, dbType: DbType.String);
                    dp.Add("inimei", value: model.IMEI, dbType: DbType.String);
                    dp.Add("incarton_label", value: model.carton_label, dbType: DbType.String);
                    dp.Add("inentry_no", value: model.EntryNo, dbType: DbType.String);
                    dp.Add("inflag", value: model.flag, dbType: DbType.Int64);

                    dp.Add("rspcode", dbType: DbType.String, direction: ParameterDirection.Output);
                    dp.Add("rspmsg", dbType: DbType.String, direction: ParameterDirection.Output);


                    connection.Open();
                    string ret = connection.Query<string>("insert_NewPackage", dp, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    //string rspjson = dp.Get<string>("rspdata");
                    returnResponse.ResponseCode = dp.Get<string>("rspcode");
                    returnResponse.ResponseMessage = dp.Get<string>("rspmsg");
                }

                return Ok(returnResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("ShowCountApi1")]
        public IActionResult ShowCountAPI1()
        {
            List<CountDSN> modelDataBinds = new List<CountDSN>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    var dp = new Dapper.DynamicParameters();
                    // dp.Add("incartoon_id", value: data.count_dsn, dbType: DbType.String);
                    modelDataBinds = connection.Query<CountDSN>("PackaesDetails", dp, commandType: CommandType.StoredProcedure).ToList();

                    if (modelDataBinds.Count == 0)
                    {
                        return NotFound(); // Return 404 if no data is found
                    }

                    return Ok(modelDataBinds); // Return data if found
                }
                catch (Exception ex)
                {
                    // Log the exception for troubleshooting
                    Console.WriteLine(ex);
                    return StatusCode(500, "An error occurred while processing the request");
                }
            }
        }

        [HttpPost("packagentry")]
        public IActionResult packagentry([FromBody] PACKAGEAPI_MODEL jobject)
        {
            List<CountPackage> modelDataBinds = new List<CountPackage>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    var dp = new Dapper.DynamicParameters();
                    dp.Add("varentry_no", value: jobject.EntryNo, dbType: DbType.String);
                    modelDataBinds = connection.Query<CountPackage>("packaesEntry", dp, commandType: CommandType.StoredProcedure).ToList();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "An error occurred while processing the request");
                }


            }

            return Ok(modelDataBinds);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost("NewPackage")]
        public IActionResult NewPackage(NewPacakgeModel model)
        {
            try
            {
                ReturnResponse returnResponse = new ReturnResponse();
                using (var connection = new NpgsqlConnection(_connectionString))
                {

                    var dp = new Dapper.DynamicParameters();
                    dp.Add("inmodel_name", value: model.Model_Name, dbType: DbType.String);
                    dp.Add("incarton_label", value: model.carton_label, dbType: DbType.String);
                    dp.Add("inentry_no", value: model.EntryNo, dbType: DbType.String);

                    dp.Add("rspcode", dbType: DbType.String, direction: ParameterDirection.Output);
                    dp.Add("rspmsg", dbType: DbType.String, direction: ParameterDirection.Output);


                    connection.Open();
                    string ret = connection.Query<string>("NewPackage", dp, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    //string rspjson = dp.Get<string>("rspdata");
                    returnResponse.ResponseCode = dp.Get<string>("rspcode");
                    returnResponse.ResponseMessage = dp.Get<string>("rspmsg");
                }

                return Ok(returnResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("InsertDsn")]
        public IActionResult InsertDsn(InserDSNModel model)
        {
            try
            {
                ReturnResponse returnResponse = new ReturnResponse();
                using (var connection = new NpgsqlConnection(_connectionString))
                {

                    var dp = new Dapper.DynamicParameters();
                    dp.Add("indsn", value: model.DSN, dbType: DbType.String);
                    dp.Add("inimei", value: model.IMEI, dbType: DbType.String);
                    dp.Add("inentry_no", value: model.EntryNo, dbType: DbType.String);
                    dp.Add("incarton_label", value: model.Carton_label, dbType: DbType.String);

                    dp.Add("rspcode", dbType: DbType.String, direction: ParameterDirection.Output);
                    dp.Add("rspmsg", dbType: DbType.String, direction: ParameterDirection.Output);


                    connection.Open();
                    string ret = connection.Query<string>("insert_dsn_imei", dp, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    //string rspjson = dp.Get<string>("rspdata");
                    returnResponse.ResponseCode = dp.Get<string>("rspcode");
                    returnResponse.ResponseMessage = dp.Get<string>("rspmsg");
                }

                return Ok(returnResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("DSNShow")]
        public IActionResult DSNShow([FromBody] DSNAPI_MODEL jobject)
        {
            List<DSNShowModel> modelDataBinds = new List<DSNShowModel>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    var dp = new Dapper.DynamicParameters();
                    dp.Add("entry_no_param", value: jobject.EntryNo, dbType: DbType.String);
                    dp.Add("varcarton", value: jobject.carton_label, dbType: DbType.String);
                    modelDataBinds = connection.Query<DSNShowModel>("dsnshow", dp, commandType: CommandType.StoredProcedure).ToList();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "An error occurred while processing the request");
                }


            }

            return Ok(modelDataBinds);
        }

        [HttpPost("ListView")]
        public IActionResult ListView([FromBody] PACKAGEAPI_MODEL jobject)
        {
            List<ListView> modelDataBinds = new List<ListView>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    var dp = new Dapper.DynamicParameters();
                    dp.Add("varentry_no", value: jobject.EntryNo, dbType: DbType.String);
                    modelDataBinds = connection.Query<ListView>("packaesDetails", dp, commandType: CommandType.StoredProcedure).ToList();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "An error occurred while processing the request");
                }


            }

            return Ok(modelDataBinds);
        }

        [HttpGet]
        [Route("DownloadMP3ZipFile")]
        public IActionResult DownloadMP3ZipFile()
        {
            string FullFilePath = "F:\\RND_TEAM_AREA\\GEN.zip";
            //Request log
            //string FullFilePath = "D:\\AISINO\\SoundFiles\\GEN.zip";

            return new PhysicalFileResult(FullFilePath, "application/octet-stream") { FileDownloadName = "GEN.zip" };
        }

        [HttpPost]
        [Route("DownloadMP3Zip")]
        public IActionResult DownloadMP3Zip()
        {
            string FullFilePath = "F:\\RND_TEAM_AREA\\GEN.zip";
            //Request log
            //string FullFilePath = "D:\\AISINO\\SoundFiles\\GEN.zip";

            return new PhysicalFileResult(FullFilePath, "application/octet-stream") { FileDownloadName = "GEN.zip" };
        }

        [HttpPost("LastMsg")]
        public IActionResult LastMsg(LastTransation lastTransation)
        {
            ReturnResponseLastTran returnResponseLastTran = new ReturnResponseLastTran();
            returnResponseLastTran.ResponseCode = "00";
            returnResponseLastTran.ResponseMessage="Success";
            returnResponseLastTran.LastTransaction = "EN|QR|44.55|||ICIpeklemaop6skdwsaalg6p2r1fqqrhwoh|stan:jjksdf|rrn:kjkf|TIME:05-03-24 19:54";
            return Ok(returnResponseLastTran);
        }

        [HttpPost("DemoRKI")]
        public async Task<IActionResult> DemoRKIAsync(DEMORKI demoRKI)
        {
            byte[] derPublicKey = HexToDer(demoRKI.PublicModulus);
            string publicKey = Convert.ToBase64String(derPublicKey);


            string apiUrl = "http://20.198.99.93:12001/dev/rkiservice/RKI/RKI";

            // JSON payload
           /* string jsonPayload = @"
            {
                ""type"": 0,
                ""TID"": ""IQ000001"",
                ""MID"": ""IQ0000000000001"",
                ""SerialNo"": ""00060000314"",
                ""RSAPublicKey"": ""MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAjyKcxKWklf8MjbeUCLxjcpd7BYBTmXh6iPYLYA6DcrMeoAaD6FYS2JfBkG3Ltlm2U6cu1jFqLruJxCZwgqYLPBntDI5wGjwygXo6bhv2FQ4IP1k2BrDhH2YJsBVrozGkWzM7m5suARhbeKJqqzeerqSxfulET5g36hQkMxJfuNdZJ8yVL1/9XQFalDLlFaL2te9CEjsXirjWMCCgNptyyRursQMj6rUypslfGfiuBnBlzrV/Kks0Qp8wBFvZKTXpiYegVa6XigpAZAxj0XOhm4o6Ttlr86lNDf5x1rfzsdHvzk7J5bjQjaBcGlQXuahiKJjVknh4K83F33Lv2NFdEwIDAQAB""
            }";*/

            RKIPayload rKIPayload = new RKIPayload();
            rKIPayload.Type = 0;
            rKIPayload.TID = "IQ000001";
            rKIPayload.MID = "IQ0000000000001";
            rKIPayload.SerialNo = "00060000314";
            rKIPayload.RSAPublicKey = publicKey;

            string jsonPayload = JsonConvert.SerializeObject(rKIPayload, Formatting.Indented);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Create the request content
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Send the POST request
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Check if request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and display the response
                        string responseContent = await response.Content.ReadAsStringAsync();

                        RKIObject rkiResponse = JsonConvert.DeserializeObject<RKIObject>(responseContent);
                        //Console.WriteLine("Response:");
                        //Console.WriteLine(responseContent);
                        byte[] byteArray = StringToByteArray(rkiResponse.Key);

                        string ipek= Encoding.ASCII.GetString(byteArray);
                        string stringWithoutFirst6Chars = ipek.Substring(6);
                         stringWithoutFirst6Chars = "QxuOlSD8LQithBxZtIkqVv8irF05vQ9W8y5BY63F/pHO6Y+A1G8a3hDVebXRNshopCFOsDmCYLyLRTF4vf7Jmv75aWr+7BZLhSzS1BqdTbr9VKHUQD920QP0czBf6tOSQ/RiHPaFvoNzvaLq71WVTlsFGRYbyrgMs6+C0Qawg45uDom/I0FXBAi2vJoJRg7pWhA6ELQgqdyiNhPagO93dMPBJsusvVjB2egBwo12NFsDO+gDVelYnpV0PZeLy4C+k6dzRec7Uyg/lbTK0wDATrRC4Y/4J8QvXKLc7/tYQMCfhVP3TNgUb0nF24rIqXKvPQUThcDURJD845qnNoVrvg==";

                        byte[] decodedBytes = Convert.FromBase64String(stringWithoutFirst6Chars);

                        // Convert bytes to ASCII representation
                        string asciiText = BytesToASCII(decodedBytes);
                        //string asciiText1 = ByteArrayToHexString(decodedBytes);
                        ReturnRKI returnRKI = new ReturnRKI();
                        returnRKI.Key = asciiText;
                        returnRKI.KSN=rkiResponse.KSN;
                        returnRKI.RspCode= rkiResponse.RspCode;
                        returnRKI.RspDesc = "";
                        return Ok(returnRKI);
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return Ok(demoRKI);
        }

        [HttpPost("DemoRKI_UsingAES")]
        public async Task<IActionResult> DemoRKI_UsingAESAsync(RKIUsingAES demoRKI)
        {
            string apiUrl = "http://20.198.99.93:12001/dev/rkiservice/RKI/RKI";
            string publicKey = string.Empty;
            string privateKey= string.Empty;
            byte[] privateKeyDer;
            string publicKeyPem;
            string privateKeyPem;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                privateKeyPem = ToPem(rsa.ExportParameters(true), true);

                // Export public key to PEM format
                 publicKeyPem = ToPem(rsa.ExportParameters(false), false);
            }
            privateKeyPem = privateKeyPem.TrimEnd('\r', '\n');
            RKIPayload rKIPayload = new RKIPayload();
            rKIPayload.Type = 0;
            rKIPayload.TID = demoRKI.TID;
            rKIPayload.MID = demoRKI.MID;
            rKIPayload.SerialNo = demoRKI.Dsn;
            rKIPayload.RSAPublicKey = publicKeyPem.TrimEnd('\r', '\n');
            //string publicKeyPemTrimmed = publicKeyPem.TrimEnd('\r', '\n');
            string jsonPayload = JsonConvert.SerializeObject(rKIPayload);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Create the request content
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Send the POST request
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Check if request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and display the response
                        string responseContent = await response.Content.ReadAsStringAsync();

                        RKIObject rkiResponse = JsonConvert.DeserializeObject<RKIObject>(responseContent);
                        //Console.WriteLine("Response:");
                        //Console.WriteLine(responseContent);
                        byte[] byteArray = StringToByteArray(rkiResponse.Key);

                        string ipek = Encoding.ASCII.GetString(byteArray);
                        string stringWithoutFirst6Chars = ipek.Substring(6);

                        //byte[] IPEKbyteArray = StringToByteArray(stringWithoutFirst6Chars);
                        privateKeyDer = Convert.FromBase64String(privateKeyPem);
                        byte[]  IPEKbyteArray = Convert.FromBase64String(stringWithoutFirst6Chars);
                        string decryptedData = DecryptData(IPEKbyteArray, privateKeyDer);

                        byte[] aesKey = GenerateRandomAesKey();

                        // Data to encrypt
                        //string originalData = "Hello, AES encryption!";

                        // Encrypt the data
                        byte[] encryptedData = EncryptAES(decryptedData, aesKey);

                        string encIPEK = Convert.ToBase64String(encryptedData);
                        string HexData = ByteArrayToHexString(encryptedData);
                        // Print the encrypted data
                        //Console.WriteLine("Encrypted data (Base64): " + Convert.ToBase64String(encryptedData));

                        ReturnRKI returnRKI = new ReturnRKI();
                       // returnRKI.IPEK = HexData;
                        return Ok(returnRKI);
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return Ok(demoRKI);
        }


        [HttpPost("AesEncRKI")]
        public IActionResult AesEncRKI(RKIUsingAES demoRKI)
        {
            AesReturnRSP aesReturnRSPaes = new AesReturnRSP();

            //byte[] aesKey = GenerateAESKey();
            byte[] aesKey =StringToByteArray(demoRKI.AesKey);
            // Print AES key in hexadecimal format
            //string aesKeyHex = ByteArrayToHexString(aesKey);
            string aesKeyHex = demoRKI.AesKey;
            Console.WriteLine("AES Key (Hex): " + aesKeyHex);

            // Encrypt data using AES key
            string plaintext = "DEEED1C263829D71EB393B7AEC70CC78";
            byte[] encryptedData = EncryptAES(Encoding.UTF8.GetBytes(plaintext), aesKey);

            aesReturnRSPaes.IPEK= Convert.ToHexString(encryptedData);
            aesReturnRSPaes.IpekLen = aesReturnRSPaes.IPEK.Length.ToString();
            aesReturnRSPaes.AesKey = aesKeyHex;
            aesReturnRSPaes.KeyLen = "256";
            aesReturnRSPaes.KSN = "FFFF6986498B0CE00000";
            return Ok(aesReturnRSPaes);
        }

        [HttpGet]
        [Route("DownloadMP3File")]
        public IActionResult DownloadZipFile()
        {
            string FullFilePath = "F:\\RND_TEAM_AREA\\welcome.mp3";
            //Request log
            //string FullFilePath = "D:\\AISINO\\SoundFiles\\GEN.zip";

            return new PhysicalFileResult(FullFilePath, "application/octet-stream") { FileDownloadName = "welcome.mp3" };
        }

        [HttpPost("DeviceLog")]
        public object DeviceLog([FromBody] Tms_LOG jobject)
        {
            string json = JsonConvert.SerializeObject(jobject);
            string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            // Write the JSON to a text file
            string filePath = @"F:\RND_TEAM_AREA\TMSLOG.txt";
            //string filePath = @"D:\DeviceOnOffStatus.txt";
            //System.IO.File.WriteAllText(filePath, json);


            if (System.IO.File.Exists(filePath))
            {
                // Append the JSON to the existing file
                System.IO.File.AppendAllText(filePath, "**********" + dateTimeStamp + "**********" + Environment.NewLine);
                System.IO.File.AppendAllText(filePath, json + Environment.NewLine);
                System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
            }
            else
            {
                // If the file doesn't exist, create it and write the JSON
                System.IO.File.WriteAllText(filePath, json);
            }


            string response = "00|Success";
            return Ok(response);
        }

        [HttpPost("AzureLog")]
        public object AzureLog([FromBody] AzureLog jobject)
        {
            if (HttpContext.Request.Headers.TryGetValue("Ocp-Apim-Subscription-Key", out var headerValue))
            {
                string testHeaderValue = headerValue.ToString();

                if (headerValue == "c6328cfd04f44b089e3310214cef85b9")
                {
                    string json = JsonConvert.SerializeObject(jobject);
                    string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                    // Write the JSON to a text file
                    string filePath = @"D:\RND_TEAM_AREA\TMSLOG.txt";
                    //string filePath = @"D:\DeviceOnOffStatus.txt";
                    //System.IO.File.WriteAllText(filePath, json);


                    if (System.IO.File.Exists(filePath))
                    {
                        // Append the JSON to the existing file
                        System.IO.File.AppendAllText(filePath, "**********" + dateTimeStamp + "**********" + Environment.NewLine);
                        System.IO.File.AppendAllText(filePath, json + Environment.NewLine);
                        System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
                    }
                    else
                    {
                        // If the file doesn't exist, create it and write the JSON
                        System.IO.File.WriteAllText(filePath, json);
                    }
                }
                else 
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
            //string response = "00|Success";
            return Ok();
        }

        [HttpPost("OTA_Ack")]
        public object OTA_Ack([FromBody] OTA_Ack jobject)
        {
            if (HttpContext.Request.Headers.TryGetValue("Ocp-Apim-Subscription-Key", out var headerValue))
            {
                string testHeaderValue = headerValue.ToString();

                if (headerValue == "c6328cfd04f44b089e3310214cef85b9")
                {
                    string json = JsonConvert.SerializeObject(jobject);
                    string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                    // Write the JSON to a text file
                    string filePath = @"D:\RND_TEAM_AREA\OTA_ACK_LOG.txt";
                    //string filePath = @"D:\DeviceOnOffStatus.txt";
                    //System.IO.File.WriteAllText(filePath, json);


                    if (System.IO.File.Exists(filePath))
                    {
                        // Append the JSON to the existing file
                        System.IO.File.AppendAllText(filePath, "**********" + dateTimeStamp + "**********" + Environment.NewLine);
                        System.IO.File.AppendAllText(filePath, json + Environment.NewLine);
                        System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
                    }
                    else
                    {
                        // If the file doesn't exist, create it and write the JSON
                        System.IO.File.WriteAllText(filePath, json);
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
            //string response = "00|Success";
            return Ok();
        }

        [HttpPost("EventAckLog")]
        public object EventAckLog([FromBody] EventAckobject jobject)
        {
            if (HttpContext.Request.Headers.TryGetValue("Ocp-Apim-Subscription-Key", out var headerValue))
            {
                string testHeaderValue = headerValue.ToString();

                if (headerValue == "c6328cfd04f44b089e3310214cef85b9")
                {
                    string json = JsonConvert.SerializeObject(jobject);
                    string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                    // Write the JSON to a text file
                    string filePath = @"D:\RND_TEAM_AREA\EVENTLOG.txt";
                    //string filePath = @"D:\DeviceOnOffStatus.txt";
                    //System.IO.File.WriteAllText(filePath, json);


                    if (System.IO.File.Exists(filePath))
                    {
                        // Append the JSON to the existing file
                        System.IO.File.AppendAllText(filePath, "**********" + dateTimeStamp + "**********" + Environment.NewLine);
                        System.IO.File.AppendAllText(filePath, json + Environment.NewLine);
                        System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
                    }
                    else
                    {
                        // If the file doesn't exist, create it and write the JSON
                        System.IO.File.WriteAllText(filePath, json);
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
            //string response = "00|Success";
            return Ok();
        }

        [HttpGet]
        [Route("DownloadWAVFile")]
        public IActionResult DownloadWAVFile()
        {
            string FullFilePath = "F:\\RND_TEAM_AREA\\glal.wav";
            //Request log
            //string FullFilePath = "D:\\AISINO\\SoundFiles\\GEN.zip";

            return new PhysicalFileResult(FullFilePath, "application/octet-stream") { FileDownloadName = "glal.wav" };
        }

        [HttpGet]
        [Route("DownloadBinFile")]
        public IActionResult DownloadBinFile()
        {
            string FullFilePath = "D:\\RND_TEAM_AREA\\App.bin";
            //string FullFilePath = "D:\\CS55\\ZIPFILEOFDFOTABIN\\App.bin";
            //Request log
            //string FullFilePath = "D:\\AISINO\\SoundFiles\\GEN.zip";

            return new PhysicalFileResult(FullFilePath, "application/octet-stream") { FileDownloadName = "App.bin" };
        }

        [HttpGet]
        [Route("DownloadZipAppFile")]
        public IActionResult DownloadZipAppFile()
        {
            string FullFilePath = "D:\\RND_TEAM_AREA\\files.zip";
            //Request log
            //string FullFilePath = "D:\\AISINO\\SoundFiles\\GEN.zip";

            return new PhysicalFileResult(FullFilePath, "application/octet-stream") { FileDownloadName = "files.zip" };
        }
        /*
        [HttpPost]
        [Route("DownloadMultipleWavFile")]
        public IActionResult DownloadMultipleWavFile(FilenameClass filename)
        {
            // Assuming the directory is the fixed path and filename is passed as a parameter.
            string directoryPath = "F:\\RND_TEAM_AREA\\HI";
            string fullFilePath = Path.Combine(directoryPath, filename.Filename);

            // Check if the file exists
            if (!System.IO.File.Exists(fullFilePath))
            {
                return NotFound(); // Return a 404 Not Found if the file doesn't exist
            }
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            // Return the file as a download
            return new PhysicalFileResult(fullFilePath, "application/octet-stream") { FileDownloadName = filename.Filename };
        }*/

        [HttpPost]
        [Route("DownloadimgFile")]
        public HttpResponseMessage DownloadimgFile()
        {
            // Path to the file you want to return
            var filePath = @"C:\path\to\your\file.pdf";
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            // Create the response message
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(fileStream)
            };

            // Set the Content-Type header explicitly
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

            // Set the Content-Disposition header to prompt the user to download the file
            /*response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "file.pdf"
            };*/

            return response;
        }

        [HttpPost]
        [Route("ImgFileDownload")]
        public IActionResult ImgFileDownload(DownImgFile downImgFile)
        {
            string json = JsonConvert.SerializeObject(downImgFile);
            string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            // Write the JSON to a text file
            string filePath = @"D:\RND_TEAM_AREA\ImgFileDownloadLOGs.txt";
            //string filePath = @"D:\DeviceOnOffStatus.txt";
            //System.IO.File.WriteAllText(filePath, json);


            if (System.IO.File.Exists(filePath))
            {
                // Append the JSON to the existing file
                System.IO.File.AppendAllText(filePath, "**********" + dateTimeStamp + "**********" + Environment.NewLine);
                System.IO.File.AppendAllText(filePath, json + Environment.NewLine);
                System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
            }
            else
            {
                // If the file doesn't exist, create it and write the JSON
                System.IO.File.WriteAllText(filePath, json);
            }
            if (HttpContext.Request.Headers.TryGetValue("Ocp-Apim-Subscription-Key", out var headerValue))
            {
                string testHeaderValue = headerValue.ToString();

                if (headerValue == "c6328cfd04f44b089e3310214cef85b9")
                {
                    string FullFilePath = "D:\\RND_TEAM_AREA\\App.img";
                    return new PhysicalFileResult(FullFilePath, "multipart/form-data") { FileDownloadName = "App.img" };
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
                    
        }

        [HttpPost]
        [Route("AudioZipDown")]
        public IActionResult AudioZipDown(DownImgFile downImgFile)
        {
            string json = JsonConvert.SerializeObject(downImgFile);
            string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            // Write the JSON to a text file
            string filePath = @"D:\RND_TEAM_AREA\AudioZipDownLOGs.txt";

            if (System.IO.File.Exists(filePath))
            {
                // Append the JSON to the existing file
                System.IO.File.AppendAllText(filePath, "**********" + dateTimeStamp + "**********" + Environment.NewLine);
                System.IO.File.AppendAllText(filePath, json + Environment.NewLine);
                System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
            }
            else
            {
                // If the file doesn't exist, create it and write the JSON
                System.IO.File.WriteAllText(filePath, json);
            }
            if (HttpContext.Request.Headers.TryGetValue("Ocp-Apim-Subscription-Key", out var headerValue))
            {
                string testHeaderValue = headerValue.ToString();

                if (headerValue == "c6328cfd04f44b089e3310214cef85b9")
                {
                    string FullFilePath = "D:\\RND_TEAM_AREA\\file.zip";
                    return new PhysicalFileResult(FullFilePath, "multipart/form-data") { FileDownloadName = "file.zip" };
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpGet]
        [Route("ImgFileDownloadw")]
        public IActionResult ImgFileDownloadw()
        {
            string FullFilePath = "D:\\RND_TEAM_AREA\\App.img";
            return new PhysicalFileResult(FullFilePath, "multipart/form-data") { FileDownloadName = "App.img" };
        }

        [HttpPost]
        [Route("DownloadMultipleWavFile")]
        public IActionResult DownloadMultipleWavFile(FilenameClass filename)
        {
            // Assuming the directory is the fixed path and filename is passed as a parameter.
            string directoryPath = "F:\\RND_TEAM_AREA\\HI";
            string fullFilePath = Path.Combine(directoryPath, filename.Filename);

            // Check if the file exists
            if (!System.IO.File.Exists(fullFilePath))
            {
                return NotFound(); // Return a 404 Not Found if the file doesn't exist
            }

            // Return the file as a download
            return new PhysicalFileResult(fullFilePath, "application/octet-stream") { FileDownloadName = filename.Filename };
        }

        [HttpPost("BajajTxnLogs")]
        public async Task<IActionResult> BajajTxnLogs([FromBody] AzureLog jobject)
        {
            string apiUrl = "https://pay-api-uat.bajajfinserv.in/merchant-sb-ack/feed";

            // Write the JSON to a text file
            string filePath = @"D:\RND_TEAM_AREA\BajajAPILogs.txt";
            //string filePath = @"D:\EVENTLOG.txt";

            if (HttpContext.Request.Headers.TryGetValue("Ocp-Apim-Subscription-Key", out var headerValue))
            {
                string testHeaderValue = headerValue.ToString();

                if (true)//(headerValue == "c6328cfd04f44b089e3310214cef85b9")
                {
                    string json = JsonConvert.SerializeObject(jobject, Formatting.Indented);
                    string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                    if (System.IO.File.Exists(filePath))
                    {
                        // Append the JSON to the existing file
                        System.IO.File.AppendAllText(filePath, "**********" + " Calling Bajaj API : " + dateTimeStamp + "**********" + Environment.NewLine);
                        System.IO.File.AppendAllText(filePath, json + Environment.NewLine);
                        //System.IO.File.AppendAllText(filePath, "********** Bajaj API" + "**********" + Environment.NewLine);
                    }
                    else
                    {
                        // If the file doesn't exist, create it and write the JSON
                        System.IO.File.WriteAllText(filePath, json);
                    }

                    // CALL BAJAJ API
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            // Create the request content
                            var content = new StringContent(json, Encoding.UTF8, "application/json");

                            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", testHeaderValue);
                            // Send the POST request
                            HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                            
                            // Check if request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                // Read and display the response
                                string responseContent = await response.Content.ReadAsStringAsync();

                                if (System.IO.File.Exists(filePath))
                                {
                                    // Append the JSON to the existing file
                                    System.IO.File.AppendAllText(filePath, Environment.NewLine +" Response Code : " + response.StatusCode + Environment.NewLine +"Response:"+ responseContent + Environment.NewLine);
                                    System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
                                }

                                return Ok(responseContent);
                            }
                            else
                            {
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.AppendAllText(filePath, Environment.NewLine +" Response Code : " + response.StatusCode + Environment.NewLine + Environment.NewLine);
                                    System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
                                }
                                return Ok("Bajaj Api Call Failed");
                            }
                        }
                        catch (Exception ex)
                        {
                            return Ok("Bajaj Api Call Failed");
                        }
                    }

                    //END
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("BajajEventLogs")]
        public async Task<IActionResult> BajajEventLogs([FromBody] EventAckobject jobject)
        {
            string apiUrl = "https://pay-api-uat.bajajfinserv.in/merchant-sb-met/tele";

            // Write the JSON to a text file
            string filePath = @"D:\RND_TEAM_AREA\EVENTLOGbajajLogs.txt";
            //string filePath = @"D:\EVENTLOG.txt";

            if (HttpContext.Request.Headers.TryGetValue("Ocp-Apim-Subscription-Key", out var headerValue))
            {
                string testHeaderValue = headerValue.ToString();

                if (true)//(headerValue == "c6328cfd04f44b089e3310214cef85b9")
                {
                    string json = JsonConvert.SerializeObject(jobject, Formatting.Indented);
                    string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                    if (System.IO.File.Exists(filePath))
                    {
                        // Append the JSON to the existing file
                        System.IO.File.AppendAllText(filePath, "**********" + " Calling Bajaj API : " + dateTimeStamp + "**********" + Environment.NewLine);
                        System.IO.File.AppendAllText(filePath, json + Environment.NewLine);
                        //System.IO.File.AppendAllText(filePath, "********** Bajaj API" + "**********" + Environment.NewLine);
                    }
                    else
                    {
                        // If the file doesn't exist, create it and write the JSON
                        System.IO.File.WriteAllText(filePath, json);
                    }

                    // CALL BAJAJ API
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            // Create the request content
                            var content = new StringContent(json, Encoding.UTF8, "application/json");

                            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", testHeaderValue);
                            // Send the POST request
                            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                            // Check if request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                // Read and display the response
                                string responseContent = await response.Content.ReadAsStringAsync();

                                if (System.IO.File.Exists(filePath))
                                {
                                    // Append the JSON to the existing file
                                    System.IO.File.AppendAllText(filePath, Environment.NewLine + " Response Code : " + response.StatusCode + Environment.NewLine +"Response :"+ responseContent + Environment.NewLine);
                                    System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
                                }

                                return Ok(responseContent);
                            }
                            else
                            {
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.AppendAllText(filePath, Environment.NewLine + " Response Code : " + response.StatusCode + Environment.NewLine + Environment.NewLine);
                                    System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
                                }
                                return Ok("Bajaj Api Call Failed");
                            }
                        }
                        catch (Exception ex)
                        {
                            return Ok("Bajaj Api Call Failed");
                        }
                    }

                    //END
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("BajajOTAackLogs")]
        public async Task<IActionResult> BajajOTAackLogs([FromBody] OTA_Ack_bajaj jobject)
        {
            string apiUrl = jobject.AckURL;//"https://pay-api-uat.bajajfinserv.in/merchant-sb-met/tele";
            OTA_Ack_bajaj_org oTA_Ack_Bajaj_Org = new OTA_Ack_bajaj_org();
            oTA_Ack_Bajaj_Org.uuid = jobject.uuid;
            oTA_Ack_Bajaj_Org.device_id = jobject.device_id;
            oTA_Ack_Bajaj_Org.status= jobject.status;
            oTA_Ack_Bajaj_Org.reason=jobject.reason;
            // Write the JSON to a text file
            string filePath = @"D:\RND_TEAM_AREA\BajajOTA_ACK.txt";
            //string filePath = @"D:\EVENTLOG.txt";

            if (HttpContext.Request.Headers.TryGetValue("Ocp-Apim-Subscription-Key", out var headerValue))
            {
                string testHeaderValue = headerValue.ToString();

                if (true)//(headerValue == "c6328cfd04f44b089e3310214cef85b9")
                {
                    string json = JsonConvert.SerializeObject(jobject, Formatting.Indented);
                    string BajajReqjson = JsonConvert.SerializeObject(oTA_Ack_Bajaj_Org, Formatting.Indented);
                    string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                    if (System.IO.File.Exists(filePath))
                    {
                        // Append the JSON to the existing file
                        System.IO.File.AppendAllText(filePath, "**********" + " Calling Bajaj API : " + dateTimeStamp + "**********" + Environment.NewLine);
                        System.IO.File.AppendAllText(filePath, json + Environment.NewLine + "Bajaj API request :" + Environment.NewLine + BajajReqjson);
                        //System.IO.File.AppendAllText(filePath, "********** Bajaj API" + "**********" + Environment.NewLine);
                    }
                    else
                    {
                        // If the file doesn't exist, create it and write the JSON
                        System.IO.File.WriteAllText(filePath, json + Environment.NewLine + "Bajaj API request :" + Environment.NewLine + BajajReqjson);
                    }

                    // CALL BAJAJ API
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            // Create the request content
                            var content = new StringContent(BajajReqjson, Encoding.UTF8, "application/json");

                            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", testHeaderValue);
                            // Send the POST request
                            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                            // Check if request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                // Read and display the response
                                string responseContent = await response.Content.ReadAsStringAsync();

                                if (System.IO.File.Exists(filePath))
                                {
                                    // Append the JSON to the existing file
                                    System.IO.File.AppendAllText(filePath, Environment.NewLine + " Response Code : " + response.StatusCode + Environment.NewLine + "Response :" + responseContent + Environment.NewLine);
                                    System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
                                }

                                return Ok(responseContent);
                            }
                            else
                            {
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.AppendAllText(filePath, Environment.NewLine + " Response Code : " + response.StatusCode + Environment.NewLine + Environment.NewLine);
                                    System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
                                }
                                return Ok("Bajaj Api Call Failed");
                            }
                        }
                        catch (Exception ex)
                        {
                            return Ok("Bajaj Api Call Failed");
                        }
                    }

                    //END
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("BajajLANGackLogs")]
        public async Task<IActionResult> BajajLANGackLogs([FromBody] OTA_Ack_bajaj jobject)
        {
            string apiUrl = jobject.AckURL;//"https://pay-api-uat.bajajfinserv.in/merchant-sb-met/tele";
            OTA_Ack_bajaj_org oTA_Ack_Bajaj_Org = new OTA_Ack_bajaj_org();
            oTA_Ack_Bajaj_Org.uuid = jobject.uuid;
            oTA_Ack_Bajaj_Org.device_id = jobject.device_id;
            oTA_Ack_Bajaj_Org.status = jobject.status;
            oTA_Ack_Bajaj_Org.reason = jobject.reason;
            // Write the JSON to a text file
            string filePath = @"D:\RND_TEAM_AREA\BajajLangAck.txt";
            //string filePath = @"D:\EVENTLOG.txt";

            if (HttpContext.Request.Headers.TryGetValue("Ocp-Apim-Subscription-Key", out var headerValue))
            {
                string testHeaderValue = headerValue.ToString();

                if (true)//(headerValue == "c6328cfd04f44b089e3310214cef85b9")
                {
                    string json = JsonConvert.SerializeObject(jobject, Formatting.Indented);
                    string BajajReqjson = JsonConvert.SerializeObject(oTA_Ack_Bajaj_Org, Formatting.Indented);
                    string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                    if (System.IO.File.Exists(filePath))
                    {
                        // Append the JSON to the existing file
                        System.IO.File.AppendAllText(filePath, "**********" + " Calling Bajaj API : " + dateTimeStamp + "**********" + Environment.NewLine);
                        System.IO.File.AppendAllText(filePath, json + Environment.NewLine + "Bajaj API request :" + Environment.NewLine + BajajReqjson);
                        //System.IO.File.AppendAllText(filePath, "********** Bajaj API" + "**********" + Environment.NewLine);
                    }
                    else
                    {
                        // If the file doesn't exist, create it and write the JSON
                        System.IO.File.WriteAllText(filePath, json + Environment.NewLine + "Bajaj API request :" + Environment.NewLine + BajajReqjson);
                    }

                    // CALL BAJAJ API
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            // Create the request content
                            var content = new StringContent(BajajReqjson, Encoding.UTF8, "application/json");

                            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", testHeaderValue);
                            // Send the POST request
                            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                            // Check if request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                // Read and display the response
                                string responseContent = await response.Content.ReadAsStringAsync();

                                if (System.IO.File.Exists(filePath))
                                {
                                    // Append the JSON to the existing file
                                    System.IO.File.AppendAllText(filePath, Environment.NewLine + " Response Code : " + response.StatusCode + Environment.NewLine + "Response :" + responseContent + Environment.NewLine);
                                    System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
                                }

                                return Ok(responseContent);
                            }
                            else
                            {
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.AppendAllText(filePath, Environment.NewLine + " Response Code : " + response.StatusCode + Environment.NewLine + Environment.NewLine);
                                    System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
                                }
                                return Ok("Bajaj Api Call Failed");
                            }
                        }
                        catch (Exception ex)
                        {
                            return Ok("Bajaj Api Call Failed");
                        }
                    }

                    //END
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("BajajOTA_ACK")]
        public async Task<IActionResult> BajajOTA_ACK([FromBody] EventAckobject jobject)
        {
            string apiUrl = "https://pay-api-uat.bajajfinserv.in/merchant-sb-met/tele";

            // Write the JSON to a text file
            string filePath = @"D:\RND_TEAM_AREA\EVENTLOG_Bajaj.txt";
            //string filePath = @"D:\EVENTLOG.txt";

            if (HttpContext.Request.Headers.TryGetValue("Ocp-Apim-Subscription-Key", out var headerValue))
            {
                string testHeaderValue = headerValue.ToString();

                if (true)//(headerValue == "c6328cfd04f44b089e3310214cef85b9")
                {
                    string json = JsonConvert.SerializeObject(jobject, Formatting.Indented);
                    string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                    if (System.IO.File.Exists(filePath))
                    {
                        // Append the JSON to the existing file
                        System.IO.File.AppendAllText(filePath, "**********" + " Calling Bajaj API : " + dateTimeStamp + "**********" + Environment.NewLine);
                        System.IO.File.AppendAllText(filePath, json + Environment.NewLine);
                        //System.IO.File.AppendAllText(filePath, "********** Bajaj API" + "**********" + Environment.NewLine);
                    }
                    else
                    {
                        // If the file doesn't exist, create it and write the JSON
                        System.IO.File.WriteAllText(filePath, json);
                    }

                    // CALL BAJAJ API
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            // Create the request content
                            var content = new StringContent(json, Encoding.UTF8, "application/json");

                            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", testHeaderValue);
                            // Send the POST request
                            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                            // Check if request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                // Read and display the response
                                string responseContent = await response.Content.ReadAsStringAsync();

                                if (System.IO.File.Exists(filePath))
                                {
                                    // Append the JSON to the existing file
                                    System.IO.File.AppendAllText(filePath, Environment.NewLine + " Response Code : " + response.StatusCode + Environment.NewLine + "Response :" + responseContent + Environment.NewLine);
                                    System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
                                }

                                return Ok(responseContent);
                            }
                            else
                            {
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.AppendAllText(filePath, Environment.NewLine + " Response Code : " + response.StatusCode + Environment.NewLine + Environment.NewLine);
                                    System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
                                }
                                return Ok("Bajaj Api Call Failed");
                            }
                        }
                        catch (Exception ex)
                        {
                            return Ok("Bajaj Api Call Failed");
                        }
                    }

                    //END
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("BajajImgDownload")]
        public async Task<IActionResult> BajajImgDownload([FromBody] DownImgFile jobject)
        {
            //string apiUrl = "https://pay-api-uat.bajajfinserv.in/soundbox-otaupdate/soundBoxOta/firmwareChange";
            string apiUrl = jobject.DownloadURL;
            DownImgFile1 downImgFile = new DownImgFile1();
            downImgFile.device_id = jobject.device_id;
            downImgFile.uuid= jobject.uuid;
            // Write the JSON to a text file
            string filePath = @"D:\RND_TEAM_AREA\BAJAJIMGDOWN.txt";
            //string filePath = @"D:\EVENTLOG.txt";

            if (HttpContext.Request.Headers.TryGetValue("Ocp-Apim-Subscription-Key", out var headerValue))
            {
                string testHeaderValue = headerValue.ToString();

                if (true)//(headerValue == "c6328cfd04f44b089e3310214cef85b9")
                {
                    string json = JsonConvert.SerializeObject(jobject, Formatting.Indented);
                    string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string json1 = JsonConvert.SerializeObject(downImgFile, Formatting.Indented);
                    if (System.IO.File.Exists(filePath))
                    {
                        // Append the JSON to the existing file
                        System.IO.File.AppendAllText(filePath, "**********" + " Calling Bajaj API : " + dateTimeStamp + "**********" + Environment.NewLine);
                        System.IO.File.AppendAllText(filePath, json + Environment.NewLine + "BAJAJ API Req : " + Environment.NewLine + json1);
                        //System.IO.File.AppendAllText(filePath, "********** Bajaj API" + "**********" + Environment.NewLine);
                    }
                    else
                    {
                        // If the file doesn't exist, create it and write the JSON
                        System.IO.File.WriteAllText(filePath, json + Environment.NewLine + "BAJAJ API Req : " + Environment.NewLine + json1);
                    }

                    // CALL BAJAJ API
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            // Create the request content
                            var content = new StringContent(json1, Encoding.UTF8, "application/json");

                            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", testHeaderValue);
                            // Send the POST request
                            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                            // Check if request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                // Read and display the response
                                string responseContent = await response.Content.ReadAsStringAsync();
                                string size = responseContent.Length.ToString();
                                //response.Content.LoadIntoBufferAsync();
                                if (System.IO.File.Exists(filePath))
                                {
                                    // Append the JSON to the existing file
                                    System.IO.File.AppendAllText(filePath, Environment.NewLine + " Response Code : " + response.StatusCode + Environment.NewLine + "Response Length:" + size + Environment.NewLine);
                                    System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
                                }
                                return Ok(responseContent);
                            }
                            else
                            {
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.AppendAllText(filePath, Environment.NewLine + " Response Code : " + response.StatusCode + Environment.NewLine + Environment.NewLine);
                                    System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
                                }
                                return Ok("Bajaj Api Call Failed");
                            }
                        }
                        catch (Exception ex)
                        {
                            return Ok("Bajaj Api Call Failed");
                        }
                    }

                    //END
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("BajajImgDownload1")]
        public async Task<IActionResult> BajajImgDownload1([FromBody] DownImgFile jobject)
        {
            string apiUrl = jobject.DownloadURL;
            DownImgFile1 downImgFile = new DownImgFile1
            {
                device_id = jobject.device_id,
                uuid = jobject.uuid
            };
            string filePath = @"D:\RND_TEAM_AREA\BAJAJIMGDOWN.txt";
            // Define the path where you want to save the .img file
            string imgFilePath = Path.Combine(@"D:\RND_TEAM_AREA\IMGfiles", "DownloadedFile_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".img");
            string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            if (HttpContext.Request.Headers.TryGetValue("Ocp-Apim-Subscription-Key", out var headerValue))
            {
                string testHeaderValue = headerValue.ToString();

                if (headerValue == "c6328cfd04f44b089e3310214cef85b9")
                {
                    string json1 = JsonConvert.SerializeObject(downImgFile, Formatting.Indented);
                    string json = JsonConvert.SerializeObject(jobject, Formatting.Indented);

                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            // Create the request content
                            var content = new StringContent(json1, Encoding.UTF8, "application/json");

                            // Add the subscription key to the headers
                            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", testHeaderValue);

                            // Send the POST request and get the response
                            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                            if (response.IsSuccessStatusCode)
                            {
                                // Read the response content as a byte array
                                byte[] imgData = await response.Content.ReadAsByteArrayAsync();

                                // Save the byte array as a .img file
                                await System.IO.File.WriteAllBytesAsync(imgFilePath, imgData);
                                // Append the JSON to the existing file
                                if (System.IO.File.Exists(filePath))
                                {
                                    // Append the JSON to the existing file
                                    System.IO.File.AppendAllText(filePath, Environment.NewLine + "**********" + " Calling Bajaj API : " + dateTimeStamp + "**********" + Environment.NewLine);
                                    System.IO.File.AppendAllText(filePath, json + Environment.NewLine + "BAJAJ API Req : " + Environment.NewLine + json1 +  Environment.NewLine + "Response :"+ response.StatusCode + Environment.NewLine + "File Path :" + imgFilePath);
                                    //System.IO.File.AppendAllText(filePath, "********** Bajaj API" + "**********" + Environment.NewLine);
                                }
                                else
                                {
                                    // If the file doesn't exist, create it and write the JSON
                                    System.IO.File.WriteAllText(filePath, Environment.NewLine + json + Environment.NewLine + "BAJAJ API Req : " + Environment.NewLine + json1 + Environment.NewLine + "Response :" + response.StatusCode + Environment.NewLine + "File Path :" +imgFilePath);
                                }
                                return new PhysicalFileResult(imgFilePath, "multipart/form-data") { FileDownloadName = "App.img" };
                            }
                            else
                            {
                                // Log failure and return error
                                if (System.IO.File.Exists(filePath))
                                {
                                    // Append the JSON to the existing file
                                    System.IO.File.AppendAllText(filePath, Environment.NewLine + "**********" + " Calling Bajaj API : " + dateTimeStamp + "**********" + Environment.NewLine);
                                    System.IO.File.AppendAllText(filePath, json + Environment.NewLine + "BAJAJ API Req : " + Environment.NewLine + json1 + Environment.NewLine + "Response :" + response.StatusCode + Environment.NewLine + "File path : " + imgFilePath);
                                    //System.IO.File.AppendAllText(filePath, "********** Bajaj API" + "**********" + Environment.NewLine);
                                }
                                else
                                {
                                    // If the file doesn't exist, create it and write the JSON
                                    System.IO.File.WriteAllText(filePath, Environment.NewLine + json + Environment.NewLine + "BAJAJ API Req : " + Environment.NewLine + json1 + Environment.NewLine + "Response :" + response.StatusCode + Environment.NewLine + "File path : " + imgFilePath);
                                }
                                return StatusCode((int)response.StatusCode, "Failed to download file.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log exception and return error
                        System.IO.File.AppendAllText(filePath, $"Exception: {ex.Message}{Environment.NewLine}");
                        return StatusCode(500, $"Exception: {ex.Message}");
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("BajajDownlangFile")]
        public async Task<IActionResult> BajajDownlangFile([FromBody] DownImgFile jobject)
        {
            string apiUrl = jobject.DownloadURL;
            DownImgFile1 downImgFile = new DownImgFile1
            {
                device_id = jobject.device_id,
                uuid = jobject.uuid
            };
            string filePath = @"D:\RND_TEAM_AREA\LangDownLogs.txt";
            // Define the path where you want to save the .img file
            string imgFilePath = Path.Combine(@"D:\RND_TEAM_AREA\ZipFiles", "DownloadedFile_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".zip");
            string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            if (HttpContext.Request.Headers.TryGetValue("Ocp-Apim-Subscription-Key", out var headerValue))
            {
                string testHeaderValue = headerValue.ToString();

                if (headerValue == "c6328cfd04f44b089e3310214cef85b9")
                {
                    string json1 = JsonConvert.SerializeObject(downImgFile, Formatting.Indented);
                    string json = JsonConvert.SerializeObject(jobject, Formatting.Indented);

                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            // Create the request content
                            var content = new StringContent(json1, Encoding.UTF8, "application/json");

                            // Add the subscription key to the headers
                            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", testHeaderValue);

                            // Send the POST request and get the response
                            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                            if (response.IsSuccessStatusCode)
                            {
                                // Read the response content as a byte array
                                byte[] imgData = await response.Content.ReadAsByteArrayAsync();

                                // Save the byte array as a .img file
                                await System.IO.File.WriteAllBytesAsync(imgFilePath, imgData);
                                // Append the JSON to the existing file
                                if (System.IO.File.Exists(filePath))
                                {
                                    // Append the JSON to the existing file
                                    System.IO.File.AppendAllText(filePath, Environment.NewLine + "**********" + " Calling Bajaj API : " + dateTimeStamp + "**********" + Environment.NewLine);
                                    System.IO.File.AppendAllText(filePath, json + Environment.NewLine + "BAJAJ API Req : " + Environment.NewLine + json1 + Environment.NewLine + "Response :" + response.StatusCode + Environment.NewLine + "File Path :" + imgFilePath);
                                    //System.IO.File.AppendAllText(filePath, "********** Bajaj API" + "**********" + Environment.NewLine);
                                }
                                else
                                {
                                    // If the file doesn't exist, create it and write the JSON
                                    System.IO.File.WriteAllText(filePath, Environment.NewLine + json + Environment.NewLine + "BAJAJ API Req : " + Environment.NewLine + json1 + Environment.NewLine + "Response :" + response.StatusCode + Environment.NewLine + "File Path :" + imgFilePath);
                                }
                                return new PhysicalFileResult(imgFilePath, "multipart/form-data") { FileDownloadName = "Audio.zip" };
                            }
                            else
                            {
                                // Log failure and return error
                                if (System.IO.File.Exists(filePath))
                                {
                                    // Append the JSON to the existing file
                                    System.IO.File.AppendAllText(filePath, Environment.NewLine + "**********" + " Calling Bajaj API : " + dateTimeStamp + "**********" + Environment.NewLine);
                                    System.IO.File.AppendAllText(filePath, json + Environment.NewLine + "BAJAJ API Req : " + Environment.NewLine + json1 + Environment.NewLine + "Response :" + response.StatusCode + Environment.NewLine + "File path : " + imgFilePath);
                                    //System.IO.File.AppendAllText(filePath, "********** Bajaj API" + "**********" + Environment.NewLine);
                                }
                                else
                                {
                                    // If the file doesn't exist, create it and write the JSON
                                    System.IO.File.WriteAllText(filePath, Environment.NewLine + json + Environment.NewLine + "BAJAJ API Req : " + Environment.NewLine + json1 + Environment.NewLine + "Response :" + response.StatusCode + Environment.NewLine + "File path : " + imgFilePath);
                                }
                                return StatusCode((int)response.StatusCode, "Failed to download file.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log exception and return error
                        System.IO.File.AppendAllText(filePath, $"Exception: {ex.Message}{Environment.NewLine}");
                        return StatusCode(500, $"Exception: {ex.Message}");
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("ZipDownloadICICI")]
        public  object ZipDownloadICICI([FromBody] ZipDownICICI jobject)
        {
            FileDownloadResponse fileDownloadResponse= new FileDownloadResponse();
            fileDownloadResponse.Biz_Type = "F";
            fileDownloadResponse.NewLanguage = "0";
            fileDownloadResponse.DeviceId = "00001504QR700W16000";
            fileDownloadResponse.DownloadHostAckUrl = "https://dev.vizpay.in:15001/NewEntry/Lang_Ack";
            fileDownloadResponse.Request_Id = 566658;
            fileDownloadResponse.OldLanguage = "1";
            fileDownloadResponse.DownloadHostUrl = "https://dev.vizpay.in:15001/NewEntry/AudioZipDownICICI";
            // Initialize the Audio_Inf list with 5 elements
            fileDownloadResponse.Audio_Inf = new List<AudioInfo>
            {
                new AudioInfo(), // Element 0
                new AudioInfo(), // Element 1
                new AudioInfo(), // Element 2
                new AudioInfo(), // Element 3
                new AudioInfo()  // Element 4
            };

            // Assign values to the Audio_Inf elements
            fileDownloadResponse.Audio_Inf[0].Uuid = "1";
            fileDownloadResponse.Audio_Inf[0].Size = 8523333;
            fileDownloadResponse.Audio_Inf[0].Sign = "af7fdeb6b78a7f1370b63a8c302638d1339dbffed7b27f6c2182125864a58ea8";

            fileDownloadResponse.Audio_Inf[1].Uuid = "2";
            fileDownloadResponse.Audio_Inf[1].Size = 4738;
            fileDownloadResponse.Audio_Inf[1].Sign = "99223bf85353610ba60eb444ed047d609594c5b56b29d862d7ddfbd1aed8c4a9";

            fileDownloadResponse.Audio_Inf[2].Uuid = "3";
            fileDownloadResponse.Audio_Inf[2].Size = 374391;
            fileDownloadResponse.Audio_Inf[2].Sign = "f98ddeb06fe70ebba642eb49a31ae78e8cc0fd0fb2544eb689ac62027339fff0";

            fileDownloadResponse.Audio_Inf[3].Uuid = "4";
            fileDownloadResponse.Audio_Inf[3].Size = 487454;
            fileDownloadResponse.Audio_Inf[3].Sign = "ddfb1ad321250f01035c4677b51510bdb5e4ec9e17d20fa13b3ec7274c054399";

            fileDownloadResponse.Audio_Inf[4].Uuid = "5";
            fileDownloadResponse.Audio_Inf[4].Size = 7590304;
            fileDownloadResponse.Audio_Inf[4].Sign = "e221857a8da1de0054f0e1423b4a57fa0b7d3e2e47163a0029ba0f96a8bcc900";
            return fileDownloadResponse;
        }

        [HttpPost]
        [Route("AudioZipDownICICI")] 
        public IActionResult AudioZipDownICICI(zipDownReq zipDownReq)
        {
            string json = JsonConvert.SerializeObject(zipDownReq);
            string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            // Write the JSON to a text file
            string filePath = @"D:\RND_TEAM_AREA\AudioZipDownLOGsICICI.txt";

            if (System.IO.File.Exists(filePath))
            {
                // Append the JSON to the existing file
                System.IO.File.AppendAllText(filePath, "**********" + dateTimeStamp + "**********" + Environment.NewLine);
                System.IO.File.AppendAllText(filePath, json + Environment.NewLine);
                System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
            }
            else
            {
                // If the file doesn't exist, create it and write the JSON
                System.IO.File.WriteAllText(filePath, json);
            }
            
            string FullFilePath = "D:\\RND_TEAM_AREA\\ICICIzipFiles\\file" + zipDownReq.Uuid+".zip";
            return new PhysicalFileResult(FullFilePath, "multipart/form-data") { FileDownloadName = "file.zip" };

        }

        [HttpPost("Lang_Ack")]
        public object Lang_Ack([FromBody] OTA_Ack jobject)
        {
            
                
                    string json = JsonConvert.SerializeObject(jobject);
                    string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                    // Write the JSON to a text file
                    string filePath = @"D:\RND_TEAM_AREA\Lang_AckICICI.txt";
                    //string filePath = @"D:\DeviceOnOffStatus.txt";
                    //System.IO.File.WriteAllText(filePath, json);


                    if (System.IO.File.Exists(filePath))
                    {
                        // Append the JSON to the existing file
                        System.IO.File.AppendAllText(filePath, "**********" + dateTimeStamp + "**********" + Environment.NewLine);
                        System.IO.File.AppendAllText(filePath, json + Environment.NewLine);
                        System.IO.File.AppendAllText(filePath, "********** END " + "**********" + Environment.NewLine);
                    }
                    else
                    {
                        // If the file doesn't exist, create it and write the JSON
                        System.IO.File.WriteAllText(filePath, json);
                    }
                
            
            //string response = "00|Success";
            return Ok();
        }
        private void LogRequest(string filePath, string json, string json1, string dateTimeStamp)
        {
            string logEntry = $"********** Calling Bajaj API : {dateTimeStamp} **********{Environment.NewLine}{json}{Environment.NewLine}BAJAJ API Req : {Environment.NewLine}{json1}{Environment.NewLine}";
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.AppendAllText(filePath, logEntry);
            }
            else
            {
                System.IO.File.WriteAllText(filePath, logEntry);
            }
        }

        private async Task<IActionResult> HandleResponse(HttpResponseMessage response, string filePath)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            string logEntry = $"Response Code : {response.StatusCode}{Environment.NewLine}Response Length: {responseContent.Length}{Environment.NewLine}********** END **********{Environment.NewLine}";

            System.IO.File.AppendAllText(filePath, logEntry);

            if (response.IsSuccessStatusCode)
            {
                return Ok(responseContent);
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Bajaj Api Call Failed");
            }
        }


        [NonAction]
        public static byte[] GenerateAESKey()
        {
            using (AesManaged aes = new AesManaged())
            {
                // Set key size to 256 bits
                aes.KeySize = 256;
                aes.GenerateKey();
                return aes.Key;
            }
        }

        [NonAction]
        public static byte[] EncryptAES(byte[] data, byte[] key)
        {
            using (AesManaged aes = new AesManaged())
            {
                // Set key size to 256 bits
                aes.KeySize = 256;
                aes.Key = key;
                aes.Mode = CipherMode.ECB; // Set mode to ECB
                aes.Padding = PaddingMode.PKCS7; // Padding mode
                //aes.IV = IV;
                // Create encryptor
                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    // Perform encryption
                    return encryptor.TransformFinalBlock(data, 0, data.Length);
                }
            }
        }

        [NonAction]
        static byte[] HexToDer(string hexKey)

        {
            byte[] bytesKey = StringToByteArray(hexKey);

            // Create RSAParameters structure
            RSAParameters rsaParams = new RSAParameters
            {
                Modulus = bytesKey,
                Exponent = new byte[] { 0x01, 0x00, 0x01 } // Public exponent (65537)
            };

            // Create RSA object
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(rsaParams);

                // Export RSA public key to DER format
                byte[] derBytes = rsa.ExportSubjectPublicKeyInfo();

                return derBytes;
            }
        }
        [NonAction]
        static byte[] StringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        [NonAction]
        static string BytesToASCII(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        [NonAction]
        static string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:X2}", b);
            }
            return hex.ToString();
        }
        [NonAction]
        public static string DecryptData(byte[] encryptedData, byte[] privateKeyDer)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(privateKeyDer);
                byte[] decryptedBytes = rsa.Decrypt(encryptedData, false);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

        [NonAction]
        public static byte[] GenerateRandomAesKey()
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                return aes.Key;
            }
        }
        [NonAction]
        public static byte[] EncryptAES(string data, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.ECB; // Use appropriate mode for your use case
                aes.Padding = PaddingMode.PKCS7; // Specify the padding mode

                // Convert the data to bytes
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                // Encrypt the data
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
                }
            }
        }

        [NonAction]
        private static string ToPem(RSAParameters parameters, bool includePrivateParameters)
        {
            var sb = new StringBuilder();
            if (includePrivateParameters)
            {
                //sb.AppendLine("-----BEGIN RSA PRIVATE KEY-----");
                sb.AppendLine(Convert.ToBase64String(EncodePrivateKey(parameters)));
                //sb.AppendLine("-----END RSA PRIVATE KEY-----");
            }
            else
            {
                //sb.AppendLine("-----BEGIN PUBLIC KEY-----");
                sb.AppendLine(Convert.ToBase64String(EncodePublicKey(parameters)));
                //sb.AppendLine("-----END PUBLIC KEY-----");
            }
            return sb.ToString();
        }
        [NonAction]
        private static byte[] EncodePrivateKey(RSAParameters parameters)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                using (var innerWriter = new BinaryWriter(innerStream))
                {
                    EncodeIntegerBigEndian(innerWriter, new byte[] { 0x00 }); // Version
                    EncodeIntegerBigEndian(innerWriter, parameters.Modulus);
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent);
                    EncodeIntegerBigEndian(innerWriter, parameters.D);
                    EncodeIntegerBigEndian(innerWriter, parameters.P);
                    EncodeIntegerBigEndian(innerWriter, parameters.Q);
                    EncodeIntegerBigEndian(innerWriter, parameters.DP);
                    EncodeIntegerBigEndian(innerWriter, parameters.DQ);
                    EncodeIntegerBigEndian(innerWriter, parameters.InverseQ);
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }
                return stream.ToArray();
            }
        }
        [NonAction]
        private static byte[] EncodePublicKey(RSAParameters parameters)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                using (var innerWriter = new BinaryWriter(innerStream))
                {
                    innerWriter.Write((byte)0x30); // SEQUENCE
                    EncodeLength(innerWriter, 13);
                    innerWriter.Write((byte)0x06); // OBJECT IDENTIFIER
                    var rsaEncryptionOid = new byte[] { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 };
                    EncodeLength(innerWriter, rsaEncryptionOid.Length);
                    innerWriter.Write(rsaEncryptionOid);
                    innerWriter.Write((byte)0x05); // NULL
                    EncodeLength(innerWriter, 0);
                    innerWriter.Write((byte)0x03); // BIT STRING
                    using (var bitStringStream = new MemoryStream())
                    using (var bitStringWriter = new BinaryWriter(bitStringStream))
                    {
                        bitStringWriter.Write((byte)0x00); // # of unused bits
                        bitStringWriter.Write((byte)0x30); // SEQUENCE
                        using (var paramsStream = new MemoryStream())
                        using (var paramsWriter = new BinaryWriter(paramsStream))
                        {
                            EncodeIntegerBigEndian(paramsWriter, parameters.Modulus); // Modulus
                            EncodeIntegerBigEndian(paramsWriter, parameters.Exponent); // Exponent
                            var paramsLength = (int)paramsStream.Length;
                            EncodeLength(bitStringWriter, paramsLength);
                            bitStringWriter.Write(paramsStream.GetBuffer(), 0, paramsLength);
                        }
                        var bitStringLength = (int)bitStringStream.Length;
                        EncodeLength(innerWriter, bitStringLength);
                        innerWriter.Write(bitStringStream.GetBuffer(), 0, bitStringLength);
                    }
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }
                return stream.ToArray();
            }
        }
        [NonAction]
        private static void EncodeLength(BinaryWriter stream, int length)
        {
            if (length < 0x80)
            {
                stream.Write((byte)length);
            }
            else if (length <= 0xff)
            {
                stream.Write((byte)0x81);
                stream.Write((byte)length);
            }
            else
            {
                stream.Write((byte)0x82);
                stream.Write((byte)(length >> 8));
                stream.Write((byte)length);
            }
        }
        [NonAction]
        private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value)
        {
            stream.Write((byte)0x02); // INTEGER
            var valueLength = value.Length;
            EncodeLength(stream, valueLength);
            stream.Write(value.Reverse().ToArray(), 0, valueLength);
        }
    }

}




    

