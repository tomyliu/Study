using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FEIBKafkaDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ConsumerConfig config;
        ConsumerBuilder<string, object> builder;
        IConsumer<string, object> consumer;
        Thread ListenThread;

        private delegate void DelegateShowMessage(string sMessage);
        private void AddMessage(string sMessage)
        {
            if (this.InvokeRequired)
            {
                DelegateShowMessage mi = new DelegateShowMessage(AddMessage);
                this.Invoke(mi, sMessage);
            }
            else
            {
                this.txtMessage.Text += sMessage + Environment.NewLine;
            }
        }

        private void btnConsumer_Click(object sender, EventArgs e)
        {
            // 建立Consumer定義
            config = new ConsumerConfig();
            config.BootstrapServers = "localhost:9092"; // Kafka主機
            config.GroupId = "FEIBGRP1";    // 指定群組名稱
            config.AutoOffsetReset = AutoOffsetReset.Earliest;  // 最舊的訊息開始派送給 Consumer
            config.EnableAutoCommit = false;    // 提交確認模式

            // 定義訊息
            builder = new ConsumerBuilder<string, object>(config);
            builder.SetValueDeserializer(new KafkaConverter()); //設置反序列化方式
            // 建立Consumer
            consumer = builder.Build();
            // 訂閱Topic
            consumer.Subscribe("FEIBTest");//訂閱消息使用Subscribe方法
            //consumer.Assign(new TopicPartition("test", new Partition(1)));    //從指定的Partition訂閱消息使用Assign方法
            //consumer.Assign(new TopicPartitionOffset("FEIBTest", 0, new Offset(0)));    // 指定從偏移位置取得訊息

            AddMessage("Start To Comsumer");

            ListenThread = new Thread(new ThreadStart(ListenConsumer));
            ListenThread.Start();

        }
        private void ListenConsumer()
        {
            while (true)
            {
                Thread.Sleep(100);

                var result = consumer.Consume();
                
                if(result.Message.Value!=null)
                    AddMessage(result.Message.Value.ToString());
                //consumer.Commit(result); //手動提交，如果上面的EnableAutoCommit=true表示自動提交，則無需使用Commit方法

            }
        }
    }

    public class KafkaConverter : IDeserializer<object>
    {/// <summary>
     /// 反序列化位元組資料成實體資料
     /// </summary>
     /// <param name="data"></param>
     /// <param name="context"></param>
     /// <returns></returns>
        public object Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull) return null;

            var json = Encoding.UTF8.GetString(data.ToArray());
            try
            {
                return JsonConvert.DeserializeObject(json);
            }
            catch
            {
                return json;
            }
        }
    }
}
