using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentScheduler;

namespace TMall.util
{
    public class JobRegistry : Registry
    {
        public JobRegistry()
        {

            // 設置每天兩點執行訂單清理任務
            Schedule<OrdersClearJob>().ToRunEvery(1).Days().At(02, 00);

            // 設置每天淩晨1點執行推薦演算法的計算
            Schedule<RecommendCalJob>().ToRunEvery(1).Days().At(01, 00);
        }
    }
}
