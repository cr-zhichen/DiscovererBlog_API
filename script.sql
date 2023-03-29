create table discoverer_blog.Article
(
    Id              int auto_increment
        primary key,
    UserId          int          not null,
    Title           varchar(255) not null,
    Content         longtext     not null,
    MarkdownContent longtext     not null,
    Tags            varchar(255) not null,
    CreatedAt       datetime(6)  not null,
    UpdatedAt       datetime(6)  not null
);

create table discoverer_blog.ArticleHistory
(
    Id              int auto_increment
        primary key,
    ArticleId       int          not null,
    UserId          int          not null,
    Title           varchar(255) not null,
    Content         longtext     not null,
    MarkdownContent longtext     not null,
    Tags            varchar(255) not null,
    CreatedAt       datetime(6)  not null
);

create table discoverer_blog.Category
(
    Id        int auto_increment
        primary key,
    Name      varchar(255) not null,
    Articles  varchar(255) not null,
    CreatedAt datetime(6)  not null,
    UpdatedAt datetime(6)  not null
);

create table discoverer_blog.Comment
(
    Id        int auto_increment
        primary key,
    ArticleId int         not null,
    UserId    int         null,
    UserName  varchar(50) null,
    Email     longtext    null,
    ParentId  int         null,
    Content   longtext    not null,
    CreatedAt datetime(6) not null,
    UpdatedAt datetime(6) not null
);

create table discoverer_blog.Notification
(
    Id        int auto_increment
        primary key,
    UserId    int          not null,
    Content   varchar(500) not null,
    IsRead    tinyint(1)   not null,
    CreatedAt datetime(6)  not null,
    UpdatedAt datetime(6)  not null
);

create table discoverer_blog.User
(
    Id        int auto_increment
        primary key,
    Username  varchar(50)  not null,
    Password  varchar(255) not null,
    Email     varchar(255) not null,
    CreatedAt datetime(6)  not null,
    UpdatedAt datetime(6)  not null
);

create table discoverer_blog.__EFMigrationsHistory
(
    MigrationId    varchar(150) not null
        primary key,
    ProductVersion varchar(32)  not null
);


