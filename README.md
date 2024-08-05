# GraphLinq

## Project Description

GraphLinq is a C# library that allows you to create GraphQL queries using a LINQ-like syntax. This library simplifies the process of generating GraphQL queries by leveraging familiar LINQ methods, enabling you to build queries with ease and without external dependencies.

## Features

- **LINQ-like Syntax**: Generate GraphQL queries using a syntax similar to LINQ.
- **Model-based Query Creation**: Create GraphQL queries based on your models.
- **No Dependencies**: The library is self-contained with no external dependencies.

## Installation Instructions

### NuGet Package

You can install the GraphLinq library via NuGet Package Manager. Run the following command in your NuGet Package Manager Console:

```bash
Install-Package GraphLinq
```

## Usage

To use GraphLinq, follow these steps:

1. **Include the Namespace**: Ensure you include the namespace in your class file.

    ```csharp
    using GraphLinq;
    ```

2. **Create a Model**: Define your model that represents the structure of your GraphQL query.

    ```csharp
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
    ```

3. **Generate a GraphQL Query**: Use GraphLinq to create a GraphQL query from your model.

    ```csharp
    var query = GraphLinq.Query<User>()
                         .Select(u => new { u.Name, u.Age, Posts = u.Posts.Select(p => new { p.Title }) })
                         .ToString();

    Console.WriteLine(query);
    ```

    This will output a GraphQL query string based on the LINQ-like syntax you have defined.

## Contributing

Contributions are welcome! If you have suggestions for improvements or would like to report bugs, please open an issue or submit a pull request on our GitHub repository.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Authors and Acknowledgments

- **Author**: Luca Bertero
- **Contributors**: Thanks to all the contributors who have helped in improving this project.

## Project Status

GraphLinq is actively maintained and in the development stage. We are continually working on adding new features and improving the existing ones.

## Contact Information

For any questions or feedback, please contact us at [your-email@example.com](mailto:your-email@example.com).


## Future Work

- **Additional LINQ Methods**: Plan to support more LINQ methods for generating complex GraphQL queries.
- **Performance Improvements**: Optimize the query generation process for better performance.
- **Comprehensive Documentation**: Provide detailed documentation and examples for users.
