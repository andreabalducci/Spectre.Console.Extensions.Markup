namespace Spectre.Console.Extensions.Markup.Example;
internal static class MarkupExample
{
    internal static string GetSampleText() =>
        """
                ---
        __Advertisement :)__

        - __[pica](https://nodeca.github.io/pica/demo/)__ - high quality and fast image
          resize in browser.
        - __[babelfish](https://github.com/nodeca/babelfish/)__ - developer friendly
          i18n with plurals support and easy syntax.

        You will like those projects!

        ---

        # h1 Heading 8-)
        ## h2 Heading
        ### h3 Heading
        #### h4 Heading
        ##### h5 Heading
        ###### h6 Heading

        ## Horizontal Rules
        ---
        ***

        ## Typographic replacements
        Enable typographer option to see result.

        (c) (C) (r) (R) (tm) (TM) (p) (P) +-

        test.. test... test..... test?..... test!....

        !!!!!! ???? ,,  -- ---

        "Smartypants, double quotes" and 'single quotes'


        ## Emphasis

        **This is bold text**
        __This is bold text__
        *This is italic text*
        _This is italic text_
        ~~Strikethrough~~

        ## Blockquotes

        > Blockquotes can also be nested...
        >> ...by using additional greater-than signs right next to each other...
        > > > ...or with spaces between arrows.

        ## Lists

        Unordered
        + Create a list by starting a line with `+`, `-`, or `*`
        + Sub-lists are made by indenting 2 spaces:
            - Marker character change forces new list start:
                * Ac tristique libero volutpat at
                + Facilisis in pretium nisl aliquet
                - Nulla volutpat aliquam velit
        + Very easy!

        Ordered
        1. Lorem ipsum dolor sit amet
        2. Consectetur adipiscing elit
        3. Integer molestie lorem at massa

        1. You can use sequential numbers...
        1. ...or keep all the numbers as `1.`

        Start numbering with offset:
        57. foo
        1. bar

        ## Code
        Inline `code`

        Indented code
            // Some comments
            line 1 of code
            line 2 of code
            line 3 of code

        Block code "fences"

        ```
        Sample text here...
        ```

        Syntax highlighting

        ```js
        var foo = function (bar) {
          return bar++;
        };

        console.log(foo(5));
        ```

        ```csharp
        using System;

        // Main method
        public readonly record struct class Program {
            public static void Main() {
                Console.WriteLine("Hello, World!");
            }
        }
        ```

        ```json
                {
            "glossary": {
                "title": "example glossary",
                "GlossDiv": {
                    "title": "S",
                    "GlossList": {
                        "GlossEntry": {
                            "ID": "SGML",
                            "SortAs": "SGML",
                            "GlossTerm": "Standard Generalized Markup Language",
                            "Acronym": "SGML",
                            "Abbrev": "ISO 8879:1986",
                            "GlossDef": {
                                "para": "A meta-markup language, used to create markup languages such as DocBook.",
                                "GlossSeeAlso": ["GML", "XML"]
                            },
                            "GlossSee": "markup"
                        }
                    }
                }
            }
        }
        ```

        ```xml
         <?xml version="1.0"?>
         <glossary>
           <title>example glossary</title>
           <GlossDiv><title>S</title>
           <GlossList>
            <!-- GlossEntry -->
            <GlossEntry ID="SGML" SortAs="SGML">
             <GlossTerm>Standard Generalized Markup Language</GlossTerm>
             <Acronym>SGML</Acronym>
             <Abbrev>ISO 8879:1986</Abbrev>
             <GlossDef>
              <para>A meta-markup language, used to create markup languages such as DocBook.</para>
              <GlossSeeAlso OtherTerm="GML" >
              <GlossSeeAlso OtherTerm="XML" >
              <![CDATA[This is CDATA content.]]>
             </GlossDef>
             <GlossSee OtherTerm="markup"/>
            </GlossEntry>
           </GlossList>
          </GlossDiv>
         </glossary>
         ```

        ```sql
        -- Create a table with various data types and constraints
        CREATE TABLE Employees (
            EmployeeID INT PRIMARY KEY,
            FirstName NVARCHAR(50) NOT NULL,
            LastName NVARCHAR(50) NOT NULL,
            Position NVARCHAR(50),
            Department NVARCHAR(50),
            Salary DECIMAL(10, 2),
            HireDate DATE DEFAULT GETDATE(),
            Active BIT DEFAULT 1
        );

        -- Insert sample data into the Employees table
        INSERT INTO Employees (EmployeeID, FirstName, LastName, Position, Department, Salary)
        VALUES 
        (1, 'Alice', 'Smith', 'Software Engineer', 'IT', 75000.00),
        (2, 'Bob', 'Johnson', 'Data Scientist', 'Analytics', 82000.50),
        (3, 'Charlie', 'Williams', 'Product Manager', 'Marketing', 91000.25);

        -- Select data with a complex query, including a JOIN, GROUP BY, and HAVING
        SELECT 
            E.Department,
            COUNT(E.EmployeeID) AS TotalEmployees,
            AVG(E.Salary) AS AverageSalary
        FROM 
            Employees E
        WHERE 
            E.Salary > 70000
        GROUP BY 
            E.Department
        HAVING 
            AVG(E.Salary) > 80000
        ORDER BY 
            AverageSalary DESC;

        -- Update the table with a condition
        UPDATE Employees
        SET Active = 0
        WHERE HireDate < '2020-01-01' OR Position = 'Product Manager';
        ```

        ## Tables

        | Option | Description |
        | ------ | ----------- |
        | data   | path to data files to supply the data that will be passed into templates. |
        | engine | engine to be used for processing templates. Handlebars is the default. |
        | ext    | extension to be used for dest files. |

        Right aligned columns

        | Option | Description |
        | ------:| -----------:|
        | data   | path to data files to supply the data that will be passed into templates. |
        | engine | engine to be used for processing templates. Handlebars is the default. |
        | ext    | extension to be used for dest files. |


        ## Links

        [link text](http://dev.nodeca.com)

        [link with title](http://nodeca.github.io/pica/demo/ "title text!")

        Autoconverted link https://github.com/nodeca/pica (enable linkify to see)


        ## Images

        ![Minion](https://octodex.github.com/images/minion.png)
        ![Stormtroopocat](https://octodex.github.com/images/stormtroopocat.jpg "The Stormtroopocat")

        Like links, Images also have a footnote style syntax

        ![Alt text][id]

        With a reference later in the document defining the URL location:

        [id]: https://octodex.github.com/images/dojocat.jpg  "The Dojocat"


        ## Plugins

        The killer feature of `markdown-it` is very effective support of
        [syntax plugins](https://www.npmjs.org/browse/keyword/markdown-it-plugin).


        ### [Emojies](https://github.com/markdown-it/markdown-it-emoji)

        > Classic markup: :wink: :ok_hand: :cry: :avocado: :laughing: :yum:
        >
        > Shortcuts (emoticons): :-) :-( 8-) ;)

        see [how to change output](https://github.com/markdown-it/markdown-it-emoji#change-output) with twemoji.


        ### [Subscript](https://github.com/markdown-it/markdown-it-sub) / [Superscript](https://github.com/markdown-it/markdown-it-sup)

        - 19^th^
        - H~2~O


        ### [\<ins>](https://github.com/markdown-it/markdown-it-ins)

        ++Inserted text++


        ### [\<mark>](https://github.com/markdown-it/markdown-it-mark)

        ==Marked text==


        ### [Footnotes](https://github.com/markdown-it/markdown-it-footnote)

        Footnote 1 link[^first].

        Footnote 2 link[^second].

        Inline footnote^[Text of inline footnote] definition.

        Duplicated footnote reference[^second].

        [^first]: Footnote **can have markup**

            and multiple paragraphs.

        [^second]: Footnote text.


        ### [Definition lists](https://github.com/markdown-it/markdown-it-deflist)

        Term 1

        :   Definition 1
        with lazy continuation.

        Term 2 with *inline markup*

        :   Definition 2

                { some code, part of Definition 2 }

            Third paragraph of definition 2.

        _Compact style:_

        Term 1
        ~ Definition 1

        Term 2
        ~ Definition 2a
        ~ Definition 2b


        ### [Abbreviations](https://github.com/markdown-it/markdown-it-abbr)

        This is HTML abbreviation example.

        It converts "HTML", but keep intact partial entries like "xxxHTMLyyy" and so on.

        *[HTML]: Hyper Text Markup Language

        ### [Custom containers](https://github.com/markdown-it/markdown-it-container)

        ::: warning
        *here be dragons*
        :::
        
        """;
}
