using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using AGS.SwitchOperations.BusinessObjects;

namespace AGS.SwitchOperations.BusinessLogics
{
    public class SmsService
    {
        public static async Task<ClsSmsResBO> sendsms(ClsSmsReqBO objSmsRequest)
        {
            ClsSmsResBO ClsSmsResBO = new ClsSmsResBO();


            HttpClient httpClient = new HttpClient();
            string baseUrl = "https://cpsolutions.dialog.lk/api/sms/inline/send.php";
            string queryParams = "?destination=" + objSmsRequest.destination + "&q=" + objSmsRequest.q + "&message=" + objSmsRequest.message + "";

            var response = await httpClient.GetAsync(new Uri(baseUrl + queryParams));

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ClsSmsResBO.Response = content;
            }
            else
            {
                ClsSmsResBO.Response = response.StatusCode.ToString();
            }
            return ClsSmsResBO;
        }
    }

}
