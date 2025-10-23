using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AMCode.Data.UnitTests.Components.MongoDB.Models
{
    /// <summary>
    /// Test model for MongoDB unit tests
    /// </summary>
    public class TestDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("age")]
        public int Age { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; }

        [BsonElement("tags")]
        public List<string> Tags { get; set; }

        [BsonElement("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        public TestDocument()
        {
            Tags = new List<string>();
            Metadata = new Dictionary<string, object>();
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }

    /// <summary>
    /// Complex test model for advanced MongoDB testing
    /// </summary>
    public class ComplexTestDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("author")]
        public Author Author { get; set; }

        [BsonElement("comments")]
        public List<Comment> Comments { get; set; }

        [BsonElement("ratings")]
        public Dictionary<string, double> Ratings { get; set; }

        [BsonElement("publishedAt")]
        public DateTime PublishedAt { get; set; }

        [BsonElement("version")]
        public int Version { get; set; }

        public ComplexTestDocument()
        {
            Comments = new List<Comment>();
            Ratings = new Dictionary<string, double>();
            PublishedAt = DateTime.UtcNow;
            Version = 1;
        }
    }

    /// <summary>
    /// Author subdocument for complex test model
    /// </summary>
    public class Author
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("bio")]
        public string Bio { get; set; }
    }

    /// <summary>
    /// Comment subdocument for complex test model
    /// </summary>
    public class Comment
    {
        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("likes")]
        public int Likes { get; set; }
    }

    /// <summary>
    /// Test data factory for MongoDB tests
    /// </summary>
    public static class MongoTestDataFactory
    {
        public static TestDocument CreateTestDocument(string name = "Test User", int age = 25, string email = "test@example.com")
        {
            return new TestDocument
            {
                Name = name,
                Age = age,
                Email = email,
                Tags = new List<string> { "test", "mongodb", "unit-test" },
                Metadata = new Dictionary<string, object>
                {
                    { "source", "unit-test" },
                    { "priority", "high" }
                }
            };
        }

        public static List<TestDocument> CreateTestDocumentList(int count = 5)
        {
            var documents = new List<TestDocument>();
            for (int i = 0; i < count; i++)
            {
                documents.Add(CreateTestDocument($"User {i}", 20 + i, $"user{i}@example.com"));
            }
            return documents;
        }

        public static ComplexTestDocument CreateComplexTestDocument(string title = "Test Article")
        {
            return new ComplexTestDocument
            {
                Title = title,
                Content = "This is test content for MongoDB unit testing.",
                Author = new Author
                {
                    Name = "Test Author",
                    Email = "author@example.com",
                    Bio = "Test author bio"
                },
                Comments = new List<Comment>
                {
                    new Comment
                    {
                        Text = "Great article!",
                        Author = "Reader1",
                        CreatedAt = DateTime.UtcNow,
                        Likes = 5
                    },
                    new Comment
                    {
                        Text = "Very informative",
                        Author = "Reader2",
                        CreatedAt = DateTime.UtcNow,
                        Likes = 3
                    }
                },
                Ratings = new Dictionary<string, double>
                {
                    { "overall", 4.5 },
                    { "content", 4.8 },
                    { "style", 4.2 }
                }
            };
        }

        public static List<ComplexTestDocument> CreateComplexTestDocumentList(int count = 3)
        {
            var documents = new List<ComplexTestDocument>();
            for (int i = 0; i < count; i++)
            {
                documents.Add(CreateComplexTestDocument($"Article {i}"));
            }
            return documents;
        }
    }
}
