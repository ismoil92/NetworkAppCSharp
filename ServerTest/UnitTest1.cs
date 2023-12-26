using NetworkAppCSharp.Models;
using NetworkAppCSharp.Services;

namespace ServerTest
{

    public class Tests
    {

        [SetUp]
        public void Setup()
        {
            using(var ctx = new ChatContext())
            {
                ctx.Messages.RemoveRange(ctx.Messages);
                ctx.Users.RemoveRange(ctx.Users);
                ctx.SaveChanges();
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var ctx = new ChatContext())
            {
                ctx.Messages.RemoveRange(ctx.Messages);
                ctx.Users.RemoveRange(ctx.Users);
                ctx.SaveChanges();
            }
        }

        [Test]
        public async Task Test1()
        {
            var mock = new MockMessageSource();
            var srv = new Server(mock);
            mock.AddServer(srv);
            await srv.Start();

            using(var ctx = new ChatContext())
            {
                Assert.IsTrue(ctx.Users.Count() == 2, "Пользователи не созданы");

                var user1 = ctx.Users.FirstOrDefault(x => x.FullName == "Вася");
                var user2 = ctx.Users.FirstOrDefault(x => x.FullName == "Юля");

                Assert.IsNotNull(user1, "Пользователь не созданы");
                Assert.IsNotNull(user2, "Пользователь не созданы");

                Assert.IsTrue(user1.MessagesFrom!.Count == 1);
                Assert.IsTrue(user2.MessagesFrom!.Count == 1);

                Assert.IsTrue(user1.MessagesTo!.Count == 1);
                Assert.IsTrue(user2.MessagesTo!.Count == 1);

                var msg1 = ctx.Messages.FirstOrDefault(x => x.UserFrom == user1 && x.UserTo == user2);
                var msg2 = ctx.Messages.FirstOrDefault(x => x.UserFrom == user2 && x.UserTo == user1);

                Assert.That(msg2?.Text, Is.EqualTo("От Юли"));
                Assert.That(msg1?.Text, Is.EqualTo("От Васи"));

                //Assert.AreEqual("От Юли", msg2.Text);
                //Assert.AreEqual("От Васи", msg1.Text);
            }
        }

        [Test]
        public async Task Test2Client()
        {
            Client client1 = new Client("Вася", "127.0.0.1", 12345);
            Assert.IsNotNull(client1, "Клиент не создан");
        }
    }
}
