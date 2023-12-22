using Microsoft.EntityFrameworkCore;

namespace Server;

public class ChatContext : DbContext
{
    #region CONSTUCTORS
    /// <summary>
    /// Контруктор без параметров
    /// </summary>
    public ChatContext() { }
    /// <summary>
    /// Контруктор с одним параметром объект типа (DbContextOptions<ChatContext>
    /// </summary>
    /// <param name="dbc">объект типа (DbContextOptions<ChatContext></param>
    public ChatContext(DbContextOptions<ChatContext> dbc) : base(dbc) { }
    #endregion

    #region PROPERTIES
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    #endregion


    #region METHODS
    /// <summary>
    /// Переопределенный метод OnConfiguring, для подключение с серверу
    /// </summary>
    /// <param name="optionsBuilder">объект который вызывает метод, для подключение к серверу</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=HP-NETBOOK-WIN\SQLEXPRESS; Database=GB;Integrated Security=False;TrustServerCertificate=True; Trusted_Connection=True;")
            .UseLazyLoadingProxies();
    }

    /// <summary>
    /// Переопределенный метод OnModelCreating, для создание таблицы в базе данных
    /// </summary>
    /// <param name="modelBuilder">объект который создаёт таблицы в базе данных</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(x => x.Id).HasName("userPk");
            entity.HasIndex(x => x.FullName).IsUnique();

            entity.Property(e => e.FullName)
           .HasColumnName("FullName")
           .HasMaxLength(255)
           .IsRequired();

        });

        modelBuilder.Entity<Message>(entity =>
        {

            entity.ToTable("messages");

            entity.HasKey(x => x.MessageId).HasName("messagePk");


            entity.Property(e => e.Text)
            .HasColumnName("messageText");
            entity.Property(e => e.DateSend)
            .HasColumnName("messageData");
            entity.Property(e => e.IsSent)
            .HasColumnName("is_sent");
            entity.Property(e => e.MessageId)
            .HasColumnName("id");

            entity.HasOne(x => x.UserTo)
            .WithMany(m => m.MessagesTo)
            .HasForeignKey(x => x.UserToId)
            .HasConstraintName("messageToUserFK");
            entity.HasOne(x => x.UserFrom)
            .WithMany(m => m.MessagesFrom)
            .HasForeignKey(x => x.UserFromId)
            .HasConstraintName("messageFromUserFK");
        });
    }
    #endregion
}