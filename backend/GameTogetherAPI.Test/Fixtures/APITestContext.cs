using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameTogetherAPI.Test.Factories;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace GameTogetherAPI.Test.Fixtures
{
    public class APITestContext
    {
        public HttpClient Client { get; set; }
        public APIFactory<Program> Factory { get; set; }
    }
}
