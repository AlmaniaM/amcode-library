using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AMCode.Common.IO
{
    /// <summary>
    /// Port of .NET Framework's TextFieldParser to .NET Core.
    /// Original source: https://github.com/microsoft/referencesource/blob/master/Microsoft.VisualBasic/runtime/msvbalib/FileIO/TextFieldParser.vb
    /// Enables parsing very large delimited or fixed width field files
    /// </summary>
    /// <remarks>Taken from the repository here: https://github.com/datchung/TextFieldParserCore. It's not a very supported repository so
    /// I just copied the code since the TextFieldParser in Microsoft.VisualBasic.FileIO is pretty stable and is unlikely to
    /// change.</remarks>
    public class TextFieldParser : IDisposable
    {
        /// <summary>
        /// Creates a new TextFieldParser to parse the passed in file
        /// </summary>
        /// <param name="path">The path of the file to be parsed</param>
        /// <remarks></remarks>
        //[HostProtection(Resources = HostProtectionResource.ExternalProcessMgmt)]
        public TextFieldParser(string path)
        {
            // Default to UTF-8 and detect encoding
            initializeFromPath(path, Encoding.UTF8, true);
        }

        /// <summary>
        /// Creates a new TextFieldParser to parse the passed in file
        /// </summary>
        /// <param name="path">The path of the file to be parsed</param>
        /// <param name="defaultEncoding">The decoding to default to if encoding isn't determined from file</param>
        /// <remarks></remarks>
        //[HostProtection(Resources = HostProtectionResource.ExternalProcessMgmt)]
        public TextFieldParser(string path, Encoding defaultEncoding)
        {
            // Default to detect encoding
            initializeFromPath(path, defaultEncoding, true);
        }

        /// <summary>
        /// Creates a new TextFieldParser to parse the passed in file
        /// </summary>
        /// <param name="path">The path of the file to be parsed</param>
        /// <param name="defaultEncoding">The decoding to default to if encoding isn't determined from file</param>
        /// <param name="detectEncoding">Indicates whether or not to try to detect the encoding from the BOM</param>
        /// <remarks></remarks>
        //[HostProtection(Resources = HostProtectionResource.ExternalProcessMgmt)]
        public TextFieldParser(string path, Encoding defaultEncoding, bool detectEncoding)
        {
            initializeFromPath(path, defaultEncoding, detectEncoding);
        }

        /// <summary>
        /// Creates a new TextFieldParser to parse a file represented by the passed in stream
        /// </summary>
        /// <param name="stream"></param>
        /// <remarks></remarks>
        //[HostProtection(Resources = HostProtectionResource.ExternalProcessMgmt)]
        public TextFieldParser(Stream stream)
        {
            // Default to UTF-8 and detect encoding
            initializeFromStream(stream, Encoding.UTF8, true);
        }

        /// <summary>
        /// Creates a new TextFieldParser to parse a file represented by the passed in stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="defaultEncoding">The decoding to default to if encoding isn't determined from file</param>
        /// <remarks></remarks>
        //[HostProtection(Resources = HostProtectionResource.ExternalProcessMgmt)]
        public TextFieldParser(Stream stream, Encoding defaultEncoding)
        {
            // Default to detect encoding
            initializeFromStream(stream, defaultEncoding, true);
        }

        /// <summary>
        /// Creates a new TextFieldParser to parse a file represented by the passed in stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="defaultEncoding">The decoding to default to if encoding isn't determined from file</param>
        /// <param name="detectEncoding">Indicates whether or not to try to detect the encoding from the BOM</param>
        /// <remarks></remarks>
        //[HostProtection(Resources = HostProtectionResource.ExternalProcessMgmt)]
        public TextFieldParser(Stream stream, Encoding defaultEncoding, bool detectEncoding)
        {
            initializeFromStream(stream, defaultEncoding, detectEncoding);
        }

        /// <summary>
        /// Creates a new TextFieldParser to parse a file represented by the passed in stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="defaultEncoding">The decoding to default to if encoding isn't determined from file</param>
        /// <param name="detectEncoding">Indicates whether or not to try to detect the encoding from the BOM</param>
        /// <param name="leaveOpen">Indicates whether or not to leave the passed in stream open</param>
        /// <remarks></remarks>
        //[HostProtection(Resources = HostProtectionResource.ExternalProcessMgmt)]
        public TextFieldParser(Stream stream, Encoding defaultEncoding, bool detectEncoding, bool leaveOpen)
        {
            this.leaveOpen = leaveOpen;
            initializeFromStream(stream, defaultEncoding, detectEncoding);
        }

        /// <summary>
        /// Creates a new TextFieldParser to parse a stream or file represented by the passed in TextReader
        /// </summary>
        /// <param name="reader">The TextReader that does the reading</param>
        /// <remarks></remarks>
        //[HostProtection(Resources = HostProtectionResource.ExternalProcessMgmt)]
        public TextFieldParser(TextReader reader)
        {
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));

            readToBuffer();
        }

        /// <summary>
        /// An array of the strings that indicate a line is a comment
        /// </summary>
        /// <value>An array of comment indicators</value>
        /// <remarks>Returns an empty array if not set</remarks>
        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        public string[] CommentTokens
        {
            get => commentTokens;
            set
            {
                checkCommentTokensForWhitespace(value);
                commentTokens = value;
                needPropertyCheck = true;
            }
        }

        /// <summary>
        /// Indicates whether or not there is any data (non ignorable lines) left to read in the file
        /// </summary>
        /// <value>True if there's more data to read, otherwise False</value>
        /// <remarks>Ignores comments and blank lines</remarks>
        public bool EndOfData
        {
            get
            {
                if (endOfData)
                {
                    return endOfData;
                }

                // Make sure we're not at end of file
                if (reader == null | buffer == null)
                {
                    endOfData = true;
                    return true;
                }

                // See if we can get a data line
                if (peekNextDataLine() != null)
                {
                    return false;
                }

                endOfData = true;
                return true;
            }
        }

        /// <summary>
        /// The line to the right of the cursor.
        /// </summary>
        /// <value>The number of the line</value>
        /// <remarks>LineNumber returns the location in the file and has nothing to do with rows or fields</remarks>
        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        public long LineNumber
        {
            get
            {
                if (lineNumber != -1)
                {

                    // See if we're at the end of file
                    if ((reader.Peek() == -1) & (position == charsRead))
                    {
                        closeReader();
                    }
                }

                return lineNumber;
            }
        }

        /// <summary>
        /// Returns the last malformed line if there is one.
        /// </summary>
        /// <value>The last malformed line</value>
        /// <remarks></remarks>
        public string ErrorLine { get; private set; } = "";

        /// <summary>
        /// Returns the line number of last malformed line if there is one.
        /// </summary>
        /// <value>The last malformed line number</value>
        /// <remarks></remarks>
        public long ErrorLineNumber { get; private set; } = -1;

        /// <summary>
        /// Indicates the type of file being read, either fixed width or delimited
        /// </summary>
        /// <value>The type of fields in the file</value>
        /// <remarks></remarks>
        public FieldType TextFieldType
        {
            get => textFieldType;
            set
            {
                validateFieldTypeEnumValue(value);
                textFieldType = value;
                needPropertyCheck = true;
            }
        }

        /// <summary>
        /// Gets or sets the widths of the fields for reading a fixed width file
        /// </summary>
        /// <value>An array of the widths</value>
        /// <remarks></remarks>
        public int[] FieldWidths
        {
            get => fieldWidths;
            set
            {
                if (value != null)
                {
                    validateFieldWidthsOnInput(value);

                    // Keep a copy so we can determine if the user changes elements of the array
                    fieldWidthsCopy = (int[])value.Clone();
                }
                else
                {
                    fieldWidthsCopy = null;
                }

                fieldWidths = value;
                needPropertyCheck = true;
            }
        }

        /// <summary>
        /// Gets or sets the delimiters used in a file
        /// </summary>
        /// <value>An array of the delimiters</value>
        /// <remarks></remarks>
        public string[] Delimiters
        {
            get => delimiters;
            set
            {
                if (value != null)
                {
                    validateDelimiters(value);

                    // Keep a copy so we can determine if the user changes elements of the array
                    delimitersCopy = (string[])value.Clone();
                }
                else
                {
                    delimitersCopy = null;
                }

                delimiters = value;

                needPropertyCheck = true;

                // Force rebuilding of regex
                cachedBeginQuotesRegex = null;
            }
        }

        /// <summary>
        /// Helper function to enable setting delimiters without dimming an array
        /// </summary>
        /// <param name="delimiters">A list of the delimiters</param>
        /// <remarks></remarks>
        public void SetDelimiters(params string[] delimiters) => Delimiters = delimiters;

        /// <summary>
        /// Helper function to enable setting field widths without dimming an array
        /// </summary>
        /// <param name="fieldWidths">A list of field widths</param>
        /// <remarks></remarks>
        public void SetFieldWidths(params int[] fieldWidths) => FieldWidths = fieldWidths;

        /// <summary>
        /// Indicates whether or not leading and trailing white space should be removed when returning a field
        /// </summary>
        /// <value>True if white space should be removed, otherwise False</value>
        /// <remarks></remarks>
        public bool TrimWhiteSpace { get; set; } = true;

        /// <summary>
        /// Reads and returns the next line from the file
        /// </summary>
        /// <returns>The line read or Nothing if at the end of the file</returns>
        /// <remarks>This is data unaware method. It simply reads the next line in the file.</remarks>
        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        public string ReadLine()
        {
            if (reader == null | buffer == null)
            {
                return null;
            }

            string Line;

            // Set the method to be used when we reach the end of the buffer
            var BufferFunction = new ChangeBufferFunction(readToBuffer);

            Line = readNextLine(ref position, BufferFunction);

            if (Line == null)
            {
                finishReading();
                return null;
            }
            else
            {
                lineNumber += 1;
                return Line.TrimEnd((char)13, (char)10);
            }
        }

        /// <summary>
        /// Reads a non ignorable line and parses it into fields
        /// </summary>
        /// <returns>The line parsed into fields</returns>
        /// <remarks>This is a data aware method. Comments and blank lines are ignored.</remarks>
        public string[] ReadFields()
        {
            if (reader == null | buffer == null)
            {
                return null;
            }

            validateReadyToRead();

            switch (textFieldType)
            {
                case FieldType.FixedWidth:
                    {
                        return parseFixedWidthLine();
                    }

                case FieldType.Delimited:
                    {
                        return parseDelimitedLine();
                    }

                default:
                    {
                        Console.WriteLine("The TextFieldType is not supported");
                        break;
                    }
            }

            return null;
        }

        /// <summary>
        /// Enables looking at the passed in number of characters of the next data line without reading the line
        /// </summary>
        /// <param name="numberOfChars"></param>
        /// <returns>A string consisting of the first NumberOfChars characters of the next line</returns>
        /// <remarks>If numberOfChars is greater than the next line, only the next line is returned</remarks>
        public string PeekChars(int numberOfChars)
        {
            if (numberOfChars <= 0)
            {
                throw new ArgumentException("", nameof(numberOfChars)); //GetArgumentExceptionWithArgName["numberOfChars", ResID.MyID.TextFieldParser_NumberOfCharsMustBePositive, "numberOfChars"];
            }

            if (reader == null | buffer == null)
            {
                return null;
            }

            // If we know there's no more data return Nothing
            if (endOfData)
            {
                return null;
            }

            // Get the next line without reading it
            var Line = peekNextDataLine();

            if (Line == null)
            {
                endOfData = true;
                return null;
            }

            // Strip of end of line chars
            Line = Line.TrimEnd((char)13, (char)10);

            // If the number of chars is larger than the line, return the whole line. Otherwise
            // return the NumberOfChars characters from the beginning of the line
            if (Line.Length < numberOfChars)
            {
                return Line;
            }
            else
            {
                //StringInfo info = new StringInfo(Line);
                //return info.SubstringByTextElements(0, numberOfChars);
                return Line.Substring(0, numberOfChars);
            }
        }

        /// <summary>
        /// Reads the file starting at the current position and moving to the end of the file
        /// </summary>
        /// <returns>The contents of the file from the current position to the end of the file</returns>
        /// <remarks>This is not a data aware method. Everything in the file from the current position to the end is read</remarks>
        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        public string ReadToEnd()
        {
            if (reader == null | buffer == null)
            {
                return null;
            }

            var Builder = new System.Text.StringBuilder(buffer.Length);

            // Get the lines in the Buffer first
            Builder.Append(buffer, position, charsRead - position);

            // Add what we haven't read
            Builder.Append(reader.ReadToEnd());

            finishReading();

            return Builder.ToString();
        }

        /// <summary>
        /// Indicates whether or not to handle quotes in a csv friendly way
        /// </summary>
        /// <value>True if we escape quotes otherwise false</value>
        /// <remarks></remarks>
        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool HasFieldsEnclosedInQuotes { get; set; } = true;

        /// <summary>
        /// Closes the StreamReader
        /// </summary>
        /// <remarks></remarks>
        public void Close() => closeReader();

        /// <summary>
        /// Closes the StreamReader
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Standard implementation of IDisposable.Dispose for non sealed classes. Classes derived from
        /// TextFieldParser should override this method. After doing their own cleanup, they should call
        /// this method (MyBase.Dispose(disposing))
        /// </summary>
        /// <param name="disposing">Indicates we are called by Dispose and not GC</param>
        /// <remarks></remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    Close();
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Validates that the value being passed as an AudioPlayMode enum is a legal value
        /// </summary>
        /// <param name="value"></param>
        /// <remarks></remarks>
        private void validateFieldTypeEnumValue(FieldType value)
        {
            if ((int)value < (int)FieldType.Delimited || (int)value > (int)FieldType.FixedWidth)
            {
                throw new ArgumentException("", nameof(value)); //(paramName, (int)value, typeof(FieldType));
            }
        }

        /// <summary>
        /// Clean up following dispose pattern
        /// </summary>
        /// <remarks></remarks>
        ~TextFieldParser()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(false);
        }

        /// <summary>
        /// Closes the StreamReader
        /// </summary>
        /// <remarks></remarks>
        private void closeReader()
        {
            finishReading();
            if (reader != null)
            {
                if (!leaveOpen)
                {
                    reader.Dispose();
                }

                reader = null;
            }
        }

        /// <summary>
        /// Cleans up managed resources except the StreamReader and indicates reading is finished
        /// </summary>
        /// <remarks></remarks>
        private void finishReading()
        {
            lineNumber = -1;
            endOfData = true;
            buffer = null;
            delimiterRegex = null;
            cachedBeginQuotesRegex = null;
        }

        /// <summary>
        /// Creates a StreamReader for the passed in Path
        /// </summary>
        /// <param name="path">The passed in path</param>
        /// <param name="defaultEncoding">The encoding to default to if encoding can't be detected</param>
        /// <param name="detectEncoding">Indicates whether or not to detect encoding from the BOM</param>
        /// <remarks>We validate the arguments here for the three Public constructors that take a Path</remarks>
        private void initializeFromPath(string path, Encoding defaultEncoding, bool detectEncoding)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (defaultEncoding == null)
            {
                throw new ArgumentNullException(nameof(defaultEncoding));
            }

            var fullPath = validatePath(path);
            var fileStreamTemp = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            reader = new StreamReader(fileStreamTemp, defaultEncoding, detectEncoding);

            readToBuffer();
        }

        /// <summary>
        /// Creates a StreamReader for a passed in stream
        /// </summary>
        /// <param name="stream">The passed in stream</param>
        /// <param name="defaultEncoding">The encoding to default to if encoding can't be detected</param>
        /// <param name="detectEncoding">Indicates whether or not to detect encoding from the BOM</param>
        /// <remarks>We validate the arguments here for the three Public constructors that take a Stream</remarks>
        private void initializeFromStream(Stream stream, Encoding defaultEncoding, bool detectEncoding)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new ArgumentException("", nameof(stream)); //GetArgumentExceptionWithArgName["stream", ResID.MyID.TextFieldParser_StreamNotReadable, "stream"];
            }

            if (defaultEncoding == null)
            {
                throw new ArgumentNullException(nameof(defaultEncoding));
            }

            reader = new StreamReader(stream, defaultEncoding, detectEncoding);

            readToBuffer();
        }

        /// <summary>
        /// Gets full name and path from passed in path.
        /// </summary>
        /// <param name="path">The path to be validated</param>
        /// <returns>The full name and path</returns>
        /// <remarks>Throws if the file doesn't exist or if the path is malformed</remarks>
        private string validatePath(string path)
        {

            // Validate and get full path
            var fullPath = path; // FileSystem.NormalizeFilePath(path, "path");

            // Make sure the file exists
            if (!File.Exists(fullPath))
            {
                throw new System.IO.FileNotFoundException(fullPath);//GetResourceString[ResID.MyID.IO_FileNotFound_Path, fullPath]);
            }

            return fullPath;
        }

        /// <summary>
        /// Indicates whether or not the passed in line should be ignored
        /// </summary>
        /// <param name="line">The line to be tested</param>
        /// <returns>True if the line should be ignored, otherwise False</returns>
        /// <remarks>Lines to ignore are blank lines and comments</remarks>
        private bool ignoreLine(string line)
        {

            // If the Line is Nothing, it has meaning (we've reached the end of the file) so don't
            // ignore it
            if (line == null)
            {
                return false;
            }

            // Ignore empty or whitespace lines
            var TrimmedLine = line.Trim();
            if (TrimmedLine.Length == 0)
            {
                return true;
            }

            // Ignore comments
            if (commentTokens != null)
            {
                foreach (var Token in commentTokens)
                {
                    if (string.IsNullOrEmpty(Token))
                    {
                        continue;
                    }

                    if (TrimmedLine.StartsWith(Token, StringComparison.Ordinal))
                    {
                        return true;
                    }

                    // Test original line in case whitespace char is a comment token
                    if (line.StartsWith(Token, StringComparison.Ordinal))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Reads characters from the file into the buffer
        /// </summary>
        /// <returns>The number of Chars read. If no Chars are read, we're at the end of the file</returns>
        /// <remarks></remarks>
        private int readToBuffer()
        {
            // Set cursor to beginning of buffer
            position = 0;
            var BufferLength = buffer.Length;
            //Debug.Assert(BufferLength >= DEFAULT_BUFFER_LENGTH, "Buffer shrunk to below default");

            // If the buffer has grown, shrink it back to the default size
            if (BufferLength > DEFAULT_BUFFER_LENGTH)
            {
                BufferLength = DEFAULT_BUFFER_LENGTH;
                buffer = new char[BufferLength - 1 + 1];
            }

            // Read from the stream
            charsRead = reader.Read(buffer, 0, BufferLength);

            // Return the number of Chars read
            return charsRead;
        }

        /// <summary>
        /// Moves the cursor and all the data to the right of the cursor to the front of the buffer. It
        /// then fills the remainder of the buffer from the file
        /// </summary>
        /// <returns>The number of Chars read in filling the remainder of the buffer</returns>
        /// <remarks>
        /// This should be called when we want to make maximum use of the space in the buffer. Characters
        /// to the left of the cursor have already been read and can be discarded.
        /// </remarks>
        private int slideCursorToStartOfBuffer()
        {
            // No need to slide if we're already at the beginning
            if (position > 0)
            {
                var BufferLength = buffer.Length;
                var TempArray = new char[BufferLength - 1 + 1];
                Array.Copy(buffer, position, TempArray, 0, BufferLength - position);

                // Fill the rest of the buffer
                var CharsRead = reader.Read(TempArray, BufferLength - position, position);
                charsRead = charsRead - position + CharsRead;

                position = 0;
                buffer = TempArray;

                return CharsRead;
            }

            return 0;
        }

        /// <summary>
        /// Increases the size of the buffer. Used when we are at the end of the buffer, we need
        /// to read more data from the file, and we can't discard what we've already read.
        /// </summary>
        /// <returns>The number of characters read to fill the new buffer</returns>
        /// <remarks>This is needed for PeekChars and EndOfData</remarks>
        private int increaseBufferSize()
        {
            // Set cursor
            peekPosition = charsRead;

            // Create a larger buffer and copy our data into it
            var BufferSize = buffer.Length + DEFAULT_BUFFER_LENGTH;

            // Make sure the buffer hasn't grown too large
            if (BufferSize > maxBufferSize)
            {
                throw new InvalidOperationException();// GetInvalidOperationException[ResID.MyID.TextFieldParser_BufferExceededMaxSize];
            }

            var TempArray = new char[BufferSize - 1 + 1];

            Array.Copy(buffer, TempArray, buffer.Length);
            var CharsRead = reader.Read(TempArray, buffer.Length, DEFAULT_BUFFER_LENGTH);
            buffer = TempArray;
            charsRead += CharsRead;

            return CharsRead;
        }

        /// <summary>
        /// Returns the next line of data or nothing if there's no more data to be read
        /// </summary>
        /// <returns>The next line of data</returns>
        /// <remarks>Moves the cursor past the line read</remarks>
        private string readNextDataLine()
        {
            string Line = default;

            // Set function to use when we reach the end of the buffer
            var BufferFunction = new ChangeBufferFunction(readToBuffer);

            do
            {
                Line = readNextLine(ref position, BufferFunction);
                lineNumber += 1;
            }
            while (ignoreLine(Line));

            if (Line == null)
            {
                closeReader();
            }

            return Line;
        }

        /// <summary>
        /// Returns the next data line but doesn't move the cursor
        /// </summary>
        /// <returns>The next data line, or Nothing if there's no more data</returns>
        /// <remarks></remarks>
        private string peekNextDataLine()
        {
            string Line = default;

            // Set function to use when we reach the end of the buffer
            var BufferFunction = new ChangeBufferFunction(increaseBufferSize);

            // Slide the data to the left so that we make maximum use of the buffer
            slideCursorToStartOfBuffer();
            peekPosition = 0;

            do
            {
                Line = readNextLine(ref peekPosition, BufferFunction);
            }
            while (ignoreLine(Line));

            return Line;
        }

        /// <summary>
        /// Function to call when we're at the end of the buffer. We either re fill the buffer
        /// or change the size of the buffer
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private delegate int ChangeBufferFunction();

        /// <summary>
        /// Gets the next line from the file and moves the passed in cursor past the line
        /// </summary>
        /// <param name="Cursor">Indicates the current position in the buffer</param>
        /// <param name="ChangeBuffer">Function to call when we've reached the end of the buffer</param>
        /// <returns>The next line in the file</returns>
        /// <remarks>Returns Nothing if we are at the end of the file</remarks>
        private string readNextLine(ref int Cursor, ChangeBufferFunction ChangeBuffer)
        {
            // Check to see if the cursor is at the end of the chars in the buffer. If it is, re fill the buffer
            if (Cursor == charsRead)
            {
                if (ChangeBuffer() == 0)
                {

                    // We're at the end of the file
                    return null;
                }
            }

            StringBuilder Builder = null;
            do
            {
                var loopTo = charsRead - 1;
                // Walk through buffer looking for the end of a line. End of line can be vbLf (\n), vbCr (\r) or vbCrLf (\r\n)
                for (var i = Cursor; i <= loopTo; i++)
                {
                    var Character = buffer[i];
                    if (Character == '\r' || Character == '\n')
                    {

                        // We've found the end of a line so add everything we've read so far to the
                        // builder. We include the end of line char because we need to know what it is
                        // in case it's embedded in a field.
                        if (Builder != null)
                        {
                            Builder.Append(buffer, Cursor, i - Cursor + 1);
                        }
                        else
                        {
                            Builder = new StringBuilder(i + 1);
                            Builder.Append(buffer, Cursor, i - Cursor + 1);
                        }

                        Cursor = i + 1;

                        // See if vbLf should be added as well
                        if (Character == '\r')
                        {
                            if (Cursor < charsRead)
                            {
                                if (buffer[Cursor] == '\n')
                                {
                                    Cursor += 1;
                                    Builder.Append('\n');
                                }
                            }
                            else if (ChangeBuffer() > 0)
                            {
                                if (buffer[Cursor] == '\n')
                                {
                                    Cursor += 1;
                                    Builder.Append('\n');
                                }
                            }
                        }

                        return Builder.ToString();
                    }
                }

                // We've searched the whole buffer and haven't found an end of line. Save what we have, and read more to the buffer.
                var Size = charsRead - Cursor;
                if (Builder == null)
                {
                    Builder = new StringBuilder(Size + DEFAULT_BUILDER_INCREASE);
                }

                Builder.Append(buffer, Cursor, Size);
            }
            while (ChangeBuffer() > 0);

            return Builder.ToString();
        }

        /// <summary>
        /// Gets the next data line and parses it with the delimiters
        /// </summary>
        /// <returns>An array of the fields in the line</returns>
        /// <remarks></remarks>
        private string[] parseDelimitedLine()
        {
            var Line = readNextDataLine();
            if (Line == null)
            {
                return null;
            }

            // The line number is that of the line just read
            var CurrentLineNumber = lineNumber - 1;

            var Index = 0;
            var Fields = new System.Collections.Generic.List<string>();
            string Field = default;
            var LineEndIndex = getEndOfLineIndex(Line);

            while (Index <= LineEndIndex)
            {

                // Is the field delimited in quotes? We only care about this if
                // EscapedQuotes is True
                Match MatchResult = null;
                var QuoteDelimited = false;

                if (HasFieldsEnclosedInQuotes)
                {
                    MatchResult = beginQuotesRegex.Match(Line, Index);
                    QuoteDelimited = MatchResult.Success;
                }

                if (QuoteDelimited)
                {

                    // Move the Index beyond quote
                    Index = MatchResult.Index + MatchResult.Length;
                    // Look for the closing "
                    var EndHelper = new QuoteDelimitedFieldBuilder(delimiterWithEndCharsRegex, spaceChars);
                    EndHelper.BuildField(Line, Index);

                    if (EndHelper.MalformedLine)
                    {
                        ErrorLine = Line.TrimEnd((char)13, (char)10);
                        ErrorLineNumber = CurrentLineNumber;
                        throw new FormatException(); //MalformedLineException(GetResourceString[ResID.MyID.TextFieldParser_MalFormedDelimitedLine, CurrentLineNumber.ToString(CultureInfo.InvariantCulture)], CurrentLineNumber);
                    }

                    if (EndHelper.FieldFinished)
                    {
                        Field = EndHelper.Field;
                        Index = EndHelper.Index + EndHelper.DelimiterLength;
                    }
                    else
                    {
                        // We may have an embedded line end character, so grab next line
                        string NewLine = default;
                        int EndOfLine = default;

                        do
                        {
                            EndOfLine = Line.Length;
                            // Get the next data line
                            NewLine = readNextDataLine();

                            // If we didn't get a new line, we're at the end of the file so our original line is mal formed
                            if (NewLine == null)
                            {
                                ErrorLine = Line.TrimEnd((char)13, (char)10);
                                ErrorLineNumber = CurrentLineNumber;
                                throw new FormatException(); // MalformedLineException(GetResourceString[ResID.MyID.TextFieldParser_MalFormedDelimitedLine, CurrentLineNumber.ToString(CultureInfo.InvariantCulture)], CurrentLineNumber);
                            }

                            if ((Line.Length + NewLine.Length) > maxLineSize)
                            {
                                ErrorLine = Line.TrimEnd((char)13, (char)10);
                                ErrorLineNumber = CurrentLineNumber;
                                throw new FormatException(); // MalformedLineException(GetResourceString[ResID.MyID.TextFieldParser_MaxLineSizeExceeded, CurrentLineNumber.ToString(CultureInfo.InvariantCulture)], CurrentLineNumber);
                            }

                            Line += NewLine;
                            LineEndIndex = getEndOfLineIndex(Line);
                            EndHelper.BuildField(Line, EndOfLine);
                            if (EndHelper.MalformedLine)
                            {
                                ErrorLine = Line.TrimEnd((char)13, (char)10);
                                ErrorLineNumber = CurrentLineNumber;
                                throw new FormatException(); // MalformedLineException(GetResourceString[ResID.MyID.TextFieldParser_MalFormedDelimitedLine, CurrentLineNumber.ToString(CultureInfo.InvariantCulture)], CurrentLineNumber);
                            }
                        }
                        while (!EndHelper.FieldFinished);

                        Field = EndHelper.Field;
                        Index = EndHelper.Index + EndHelper.DelimiterLength;
                    }

                    if (TrimWhiteSpace)
                    {
                        Field = Field.Trim();
                    }

                    Fields.Add(Field);
                }
                else
                {
                    // Find the next delimiter
                    var DelimiterMatch = delimiterRegex.Match(Line, Index);
                    if (DelimiterMatch.Success)
                    {
                        Field = Line.Substring(Index, DelimiterMatch.Index - Index);

                        if (TrimWhiteSpace)
                        {
                            Field = Field.Trim();
                        }

                        Fields.Add(Field);

                        // Move the index
                        Index = DelimiterMatch.Index + DelimiterMatch.Length;
                    }
                    else
                    {
                        // We're at the end of the line so the field consists of all that's left of the line
                        // minus the end of line chars
                        Field = Line.Substring(Index).TrimEnd((char)13, (char)10);

                        if (TrimWhiteSpace)
                        {
                            Field = Field.Trim();
                        }

                        Fields.Add(Field);
                        break;
                    }
                }
            }

            return Fields.ToArray();
        }

        /// <summary>
        /// Gets the next data line and parses into fixed width fields
        /// </summary>
        /// <returns>An array of the fields in the line</returns>
        /// <remarks></remarks>
        private string[] parseFixedWidthLine()
        {
            var Line = readNextDataLine();

            if (Line == null)
            {
                return null;
            }

            // Strip off trailing carriage return or line feed
            Line = Line.TrimEnd((char)13, (char)10);

            var LineInfo = new StringInfo(Line);
            validateFixedWidthLine(LineInfo);

            var Index = 0;
            var Bound = fieldWidths.Length - 1;
            var Fields = new string[Bound + 1];
            var loopTo = Bound;
            for (var i = 0; i <= loopTo; i++)
            {
                Fields[i] = getFixedWidthField(LineInfo, Index, fieldWidths[i]);
                Index += fieldWidths[i];
            }

            return Fields;
        }

        /// <summary>
        /// Returns the field at the passed in index
        /// </summary>
        /// <param name="Line">The string containing the fields</param>
        /// <param name="Index">The start of the field</param>
        /// <param name="FieldLength">The length of the field</param>
        /// <returns>The field</returns>
        /// <remarks></remarks>
        private string getFixedWidthField(StringInfo Line, int Index, int FieldLength)
        {
            string Field;
            if (FieldLength > 0)
            {
                Field = Line.String.Substring(Index, FieldLength); //Line.SubstringByTextElements(Index, FieldLength);
            }
            else
                // Make sure the index isn't past the string
                if (Index >= Line.LengthInTextElements)
            {
                Field = string.Empty;
            }
            else
            {
                Field = Line.String.Substring(Index).TrimEnd((char)13, (char)10); //.SubstringByTextElements(Index).TrimEnd((char)13, (char)10);
            }

            if (TrimWhiteSpace)
            {
                return Field.Trim();
            }
            else
            {
                return Field;
            }
        }

        /// <summary>
        /// Gets the index of the first end of line character
        /// </summary>
        /// <param name="Line"></param>
        /// <returns></returns>
        /// <remarks>When there are no end of line characters, the index is the length (one past the end)</remarks>
        private int getEndOfLineIndex(string Line)
        {
            //Debug.Assert(Line != null, "We are parsing a Nothing");

            var Length = Line.Length;
            //Debug.Assert(Length > 0, "A blank line shouldn't be parsed");

            if (Length == 1)
            {
                //Debug.Assert((Conversions.ToString(Line[0]) != Constants.vbCr) & (Conversions.ToString(Line[0]) != Constants.vbLf), "A blank line shouldn't be parsed");
                return Length;
            }

            // Check the next to last and last char for end line characters
            if (Line[Length - 2] == '\r' | Line[Length - 2] == '\n')
            {
                return Length - 2;
            }
            else if (Line[Length - 1] == '\r' | Line[Length - 1] == '\n')
            {
                return Length - 1;
            }
            else
            {
                return Length;
            }
        }

        /// <summary>
        /// Indicates whether or not a line is valid
        /// </summary>
        /// <param name="Line">The line to be tested</param>
        /// <remarks></remarks>
        private void validateFixedWidthLine(StringInfo Line)
        {
            //Debug.Assert(Line != null, "No Line sent");

            // The only mal-formed line for fixed length fields is one that's too short            
            if (Line.LengthInTextElements < lineLength)
            {
                ErrorLine = Line.String;
                ErrorLineNumber = lineNumber - 1;
                throw new FormatException(); // MalformedLineException(GetResourceString[ResID.MyID.TextFieldParser_MalFormedFixedWidthLine, LineNumber.ToString(CultureInfo.InvariantCulture)], LineNumber);
            }
        }

        /// <summary>
        /// Determines whether or not the field widths are valid, and sets the size of a line
        /// </summary>
        /// <remarks></remarks>
        private void validateFieldWidths()
        {
            if (fieldWidths == null)
            {
                throw new InvalidOperationException(); //GetInvalidOperationException[ResID.MyID.TextFieldParser_FieldWidthsNothing];
            }

            if (fieldWidths.Length == 0)
            {
                throw new InvalidOperationException(); //GetInvalidOperationException[ResID.MyID.TextFieldParser_FieldWidthsNothing];
            }

            var WidthBound = fieldWidths.Length - 1;
            lineLength = 0;
            var loopTo = WidthBound - 1;

            // add all but the last element
            for (var i = 0; i <= loopTo; i++)
            {
                lineLength += fieldWidths[i];
            }

            // add the last field if it's greater than zero (ie not ragged).
            if (fieldWidths[WidthBound] > 0)
            {
                lineLength += fieldWidths[WidthBound];
            }
        }

        /// <summary>
        /// Checks the field widths at input.
        /// </summary>
        /// <param name="Widths"></param>
        /// <remarks>
        /// All field widths, except the last one, must be greater than zero. If the last width is
        /// less than one it indicates the last field is ragged
        /// </remarks>
        private void validateFieldWidthsOnInput(int[] Widths)
        {
            //Debug.Assert(Widths != null, "There are no field widths");

            var Bound = Widths.Length - 1;
            var loopTo = Bound - 1;
            for (var i = 0; i <= loopTo; i++)
            {
                if (Widths[i] < 1)
                {
                    throw new ArgumentException("", nameof(Widths)); //GetArgumentExceptionWithArgName["FieldWidths", ResID.MyID.TextFieldParser_FieldWidthsMustPositive, "FieldWidths"];
                }
            }
        }

        /// <summary>
        /// Validates the delimiters and creates the Regex objects for finding delimiters or quotes followed
        /// by delimiters
        /// </summary>
        /// <remarks></remarks>
        private void validateAndEscapeDelimiters()
        {
            if (delimiters == null)
            {
                throw new ArgumentException("", nameof(Delimiters)); //GetArgumentExceptionWithArgName["Delimiters", ResID.MyID.TextFieldParser_DelimitersNothing, "Delimiters"];
            }

            if (delimiters.Length == 0)
            {
                throw new ArgumentException("", nameof(Delimiters)); //GetArgumentExceptionWithArgName["Delimiters", ResID.MyID.TextFieldParser_DelimitersNothing, "Delimiters"];
            }

            var Length = delimiters.Length;

            var Builder = new StringBuilder();
            var QuoteBuilder = new StringBuilder();

            // Add ending quote pattern. It will be followed by delimiters resulting in a string like:
            // "[ ]*(d1|d2|d3)
            QuoteBuilder.Append(endQuotePattern + "(");
            var loopTo = Length - 1;
            for (var i = 0; i <= loopTo; i++)
            {
                if (delimiters[i] != null)
                {

                    // Make sure delimiter is legal
                    if (HasFieldsEnclosedInQuotes)
                    {
                        if (delimiters[i].IndexOf('"') > -1)
                        {
                            throw new InvalidOperationException(); //GetInvalidOperationException[ResID.MyID.TextFieldParser_IllegalDelimiter];
                        }
                    }

                    var EscapedDelimiter = Regex.Escape(delimiters[i]);

                    Builder.Append(EscapedDelimiter + "|");
                    QuoteBuilder.Append(EscapedDelimiter + "|");
                }
                else
                {
                    Console.WriteLine("Delimiter element is empty. This should have been caught on input");
                }
            }

            spaceChars = whitespaceCharacters;

            // Get rid of trailing | and set regex
            delimiterRegex = new Regex(Builder.ToString(0, Builder.Length - 1), REGEX_OPTIONS);
            Builder.Append("\r|\n");
            delimiterWithEndCharsRegex = new Regex(Builder.ToString(), REGEX_OPTIONS);

            // Add end of line (either cr, ln, or nothing) and set regex
            QuoteBuilder.Append("\r|\n)|\"$");
        }

        /// <summary>
        /// Checks property settings to ensure we're able to read fields.
        /// </summary>
        /// <remarks>Throws if we're not able to read fields with current property settings</remarks>
        private void validateReadyToRead()
        {
            if (needPropertyCheck | arrayHasChanged())
            {
                switch (textFieldType)
                {
                    case FieldType.Delimited:
                        {
                            validateAndEscapeDelimiters();
                            break;
                        }

                    case FieldType.FixedWidth:
                        {

                            // Check FieldWidths
                            validateFieldWidths();
                            break;
                        }

                    default:
                        {
                            Console.WriteLine("Unknown TextFieldType");
                            break;
                        }
                }

                // Check Comment Tokens
                if (commentTokens != null)
                {
                    foreach (var Token in commentTokens)
                    {
                        if (!string.IsNullOrEmpty(Token))
                        {
                            if (HasFieldsEnclosedInQuotes & (textFieldType == (int)FieldType.Delimited))
                            {
                                if (string.Compare(Token.Trim(), "\"", StringComparison.Ordinal) == 0)
                                {
                                    throw new InvalidOperationException(); //GetInvalidOperationException[ResID.MyID.TextFieldParser_InvalidComment];
                                }
                            }
                        }
                    }
                }

                needPropertyCheck = false;
            }
        }

        /// <summary>
        /// Throws if any of the delimiters contain line end characters
        /// </summary>
        /// <param name="delimiterArray">A string array of delimiters</param>
        /// <remarks></remarks>
        private void validateDelimiters(string[] delimiterArray)
        {
            if (delimiterArray == null)
            {
                return;
            }

            foreach (var delimiter in delimiterArray)
            {
                if (string.IsNullOrEmpty(delimiter))
                {
                    throw new ArgumentException("", nameof(Delimiters)); //GetArgumentExceptionWithArgName["Delimiters", ResID.MyID.TextFieldParser_DelimiterNothing, "Delimiters"];
                }

                if (delimiter.IndexOfAny(new char[] { (char)13, (char)10 }) > -1)
                {
                    throw new ArgumentException("", nameof(Delimiters)); //GetArgumentExceptionWithArgName["Delimiters", ResID.MyID.TextFieldParser_EndCharsInDelimiter];
                }
            }
        }

        /// <summary>
        /// Determines if the FieldWidths or Delimiters arrays have changed.
        /// </summary>
        /// <remarks>If the array has changed, we need to re initialize before reading.</remarks>
        private bool arrayHasChanged()
        {
            var lowerBound = 0;
            var upperBound = 0;

            switch (textFieldType)
            {
                case FieldType.Delimited:
                    {
                        // Check null cases
                        if (delimiters == null)
                        {
                            return false;
                        }

                        lowerBound = delimitersCopy.GetLowerBound(0);
                        upperBound = delimitersCopy.GetUpperBound(0);
                        var loopTo = upperBound;
                        for (var i = lowerBound; i <= loopTo; i++)
                        {
                            if ((delimiters[i] ?? "") != (delimitersCopy[i] ?? ""))
                            {
                                return true;
                            }
                        }

                        break;
                    }

                case FieldType.FixedWidth:
                    {
                        // Check null cases
                        if (fieldWidths == null)
                        {
                            return false;
                        }

                        lowerBound = fieldWidthsCopy.GetLowerBound(0);
                        upperBound = fieldWidthsCopy.GetUpperBound(0);
                        var loopTo1 = upperBound;
                        for (var i = lowerBound; i <= loopTo1; i++)
                        {
                            if (fieldWidths[i] != fieldWidthsCopy[i])
                            {
                                return true;
                            }
                        }

                        break;
                    }

                default:
                    {
                        Console.WriteLine("Unknown TextFieldType");
                        break;
                    }
            }

            return false;
        }

        /// <summary>
        /// Throws if any of the comment tokens contain whitespace
        /// </summary>
        /// <param name="tokens">A string array of comment tokens</param>
        /// <remarks></remarks>
        private void checkCommentTokensForWhitespace(string[] tokens)
        {
            if (tokens == null)
            {
                return;
            }

            foreach (var token in tokens)
            {
                if (whiteSpaceRegEx.IsMatch(token))
                {
                    throw new ArgumentException("", nameof(tokens)); //GetArgumentExceptionWithArgName["CommentTokens", ResID.MyID.TextFieldParser_WhitespaceInToken];
                }
            }
        }

        /// <summary>
        /// Gets the appropriate regex for finding a field beginning with quotes
        /// </summary>
        /// <value>The right regex</value>
        /// <remarks></remarks>
        private Regex beginQuotesRegex
        {
            get
            {
                if (cachedBeginQuotesRegex == null)
                {
                    // Get the pattern
                    var pattern = string.Format(CultureInfo.InvariantCulture, BEGINS_WITH_QUOTE, whitespacePattern);
                    cachedBeginQuotesRegex = new Regex(pattern, REGEX_OPTIONS);
                }

                return cachedBeginQuotesRegex;
            }
        }

        /// <summary>
        /// Gets the appropriate expression for finding ending quote of a field
        /// </summary>
        /// <value>The expression</value>
        /// <remarks></remarks>
        private string endQuotePattern => string.Format(CultureInfo.InvariantCulture, ENDING_QUOTE, whitespacePattern);

        /// <summary>
        /// Returns a string containing all the characters which are whitespace for parsing purposes
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        private string whitespaceCharacters
        {
            get
            {
                var builder = new StringBuilder();
                foreach (var code in whitespaceCodes)
                {
                    var spaceChar = (char)code;
                    if (!characterIsInDelimiter(spaceChar))
                    {
                        builder.Append(spaceChar);
                    }
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// Gets the character set of white-spaces to be used in a regex pattern
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        private string whitespacePattern
        {
            get
            {
                var builder = new StringBuilder();
                foreach (var code in whitespaceCodes)
                {
                    var spaceChar = (char)code;
                    if (!characterIsInDelimiter(spaceChar))
                    {
                        // Gives us something like \u00A0
                        builder.Append(@"\u" + code.ToString("X4", CultureInfo.InvariantCulture));
                    }
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// Checks to see if the passed in character is in any of the delimiters
        /// </summary>
        /// <param name="testCharacter">The character to look for</param>
        /// <returns>True if the character is found in a delimiter, otherwise false</returns>
        /// <remarks></remarks>
        private bool characterIsInDelimiter(char testCharacter)
        {
            foreach (var delimiter in delimiters)
            {
                if (delimiter.IndexOf(testCharacter) > -1)
                {
                    return true;
                }
            }

            return false;
        }

        // Indicates reader has been disposed
        private bool disposed;

        // The internal StreamReader that reads the file
        private TextReader reader;

        // An array holding the strings that indicate a line is a comment
        private string[] commentTokens = new string[] { };

        // The line last read by either ReadLine or ReadFields
        private long lineNumber = 1;

        // Flags whether or not there is data left to read. Assume there is at creation
        private bool endOfData = false;

        // Indicates what type of fields are in the file (fixed width or delimited)
        private FieldType textFieldType = FieldType.Delimited;

        // An array of the widths of the fields in a fixed width file
        private int[] fieldWidths;

        // An array of the delimiters used for the fields in the file
        private string[] delimiters;

        // Holds a copy of the field widths last set so we can respond to changes in the array
        private int[] fieldWidthsCopy;

        // Holds a copy of the field widths last set so we can respond to changes in the array
        private string[] delimitersCopy;

        // Regular expression used to find delimiters
        private Regex delimiterRegex;

        // Regex used with BuildField
        private Regex delimiterWithEndCharsRegex;

        // Options used for regular expressions
        private const RegexOptions REGEX_OPTIONS = RegexOptions.CultureInvariant;

        // Codes for whitespace as used by String.Trim excluding line end chars as those are handled separately
        private readonly int[] whitespaceCodes = new int[] { 0x9, 0xB, 0xC, 0x20, 0x85, 0xA0, 0x1680, 0x2000, 0x2001, 0x2002, 0x2003, 0x2004, 0x2005, 0x2006, 0x2007, 0x2008, 0x2009, 0x200A, 0x200B, 0x2028, 0x2029, 0x3000, 0xFEFF };

        // Regular expression used to find beginning quotes ignore spaces and tabs
        private Regex cachedBeginQuotesRegex;

        // Regular expression for whitespace
        private readonly Regex whiteSpaceRegEx = new Regex(@"\s", REGEX_OPTIONS);

        // The position of the cursor in the buffer
        private int position = 0;

        // The position of the peek cursor
        private int peekPosition = 0;

        // The number of chars in the buffer
        private int charsRead = 0;

        // Indicates that the user has changed properties so that we need to validate before a read
        private bool needPropertyCheck = true;

        // The default size for the buffer
        private const int DEFAULT_BUFFER_LENGTH = 4096;

        // This is a guess as to how much larger the string builder should be beyond the size of what
        // we've already read
        private const int DEFAULT_BUILDER_INCREASE = 10;

        // Buffer used to hold data read from the file. It holds data that must be read
        // ahead of the cursor (for PeekChars and EndOfData)
        private char[] buffer = new char[4096];

        // The minimum length for a valid fixed width line
        private int lineLength;

        // A string of the chars that count as spaces (used for csv format). The norm is spaces and tabs.
        private string spaceChars;

        // The largest size a line can be.
        private readonly int maxLineSize = 10000000;

        // The largest size the buffer can be
        private readonly int maxBufferSize = 10000000;

        // Regex pattern to determine if field begins with quotes
        private const string BEGINS_WITH_QUOTE = @"\G[{0}]*""";

        // Regex pattern to find a quote before a delimiter
        private const string ENDING_QUOTE = "\"[{0}]*";

        // Indicates passed in stream should be not be closed
        private readonly bool leaveOpen = false;
    }

    /// <summary>
    /// Enum used to indicate the kind of file being read, either delimited or fixed length
    /// </summary>
    /// <remarks></remarks>
    public enum FieldType : int
    {
        /// <summary>
        /// The file is delimited on special characters. NOTE: Changes to this enum must be reflected in ValidateFieldTypeEnumValue().
        /// </summary>
        Delimited,
        /// <summary>
        /// Values in the file are of fixed width.
        /// </summary>
        FixedWidth
    }

    /// <summary>
    /// Helper class that when passed a line and an index to a quote delimited field
    /// will build the field and handle escaped quotes
    /// </summary>
    /// <remarks></remarks>
    internal class QuoteDelimitedFieldBuilder
    {
        /// <summary>
        /// Creates an instance of the class and sets some properties
        /// </summary>
        /// <param name="DelimiterRegex">The regex used to find any of the delimiters</param>
        /// <param name="SpaceChars">Characters treated as space (usually space and tab)</param>
        /// <remarks></remarks>
        public QuoteDelimitedFieldBuilder(Regex DelimiterRegex, string SpaceChars)
        {
            delimiterRegex = DelimiterRegex;
            spaceChars = SpaceChars;
        }

        /// <summary>
        /// Indicates whether or not the field has been built.
        /// </summary>
        /// <value>True if the field has been built, otherwise False</value>
        /// <remarks>If the Field has been built, the Field property will return the entire field</remarks>
        public bool FieldFinished { get; private set; }

        /// <summary>
        /// The field being built
        /// </summary>
        /// <value>The field</value>
        /// <remarks></remarks>
        public string Field => field.ToString();

        /// <summary>
        /// The current index on the line. Used to indicate how much of the line was used to build the field
        /// </summary>
        /// <value>The current position on the line</value>
        /// <remarks></remarks>
        public int Index { get; private set; }

        /// <summary>
        /// The length of the closing delimiter if one was found
        /// </summary>
        /// <value>The length of the delimiter</value>
        /// <remarks></remarks>
        public int DelimiterLength { get; private set; }

        /// <summary>
        /// Indicates that the current field breaks the subset of csv rules we enforce
        /// </summary>
        /// <value>True if the line is malformed, otherwise False</value>
        /// <remarks>
        /// The rules we enforce are:
        /// Embedded quotes must be escaped
        /// Only space characters can occur between a delimiter and a quote
        /// </remarks>
        public bool MalformedLine { get; private set; }

        /// <summary>
        /// Builds a field by walking through the passed in line starting at StartAt
        /// </summary>
        /// <param name="Line">The line containing the data</param>
        /// <param name="StartAt">The index at which we start building the field</param>
        /// <remarks></remarks>
        public void BuildField(string Line, int StartAt)
        {
            Index = StartAt;
            var Length = Line.Length;

            while (Index < Length)
            {
                if (Line[Index] == '"')
                {

                    // Are we at the end of the file?
                    if ((Index + 1) == Length)
                    {
                        // We've found the end of the field
                        FieldFinished = true;
                        DelimiterLength = 1;

                        // Move index past end of line
                        Index += 1;
                        return;
                    }
                    // Check to see if this is an escaped quote
                    if (((Index + 1) < Line.Length) & (Line[Index + 1] == '"'))
                    {
                        field.Append('"');
                        Index += 2;
                        continue;
                    }

                    // Find the next delimiter and make sure everything between the quote and 
                    // the delimiter is ignorable
                    int limit = default;
                    var DelimiterMatch = delimiterRegex.Match(Line, Index + 1);
                    limit = !DelimiterMatch.Success ? Length - 1 : DelimiterMatch.Index - 1;
                    var loopTo = limit;
                    for (var i = Index + 1; i <= loopTo; i++)
                    {
                        if (spaceChars.IndexOf(Line[i]) < 0)
                        {
                            MalformedLine = true;
                            return;
                        }
                    }

                    // The length of the delimiter is the length of the closing quote (1) + any spaces + the length
                    // of the delimiter we matched if any
                    DelimiterLength = 1 + limit - Index;
                    if (DelimiterMatch.Success)
                    {
                        DelimiterLength += DelimiterMatch.Length;
                    }

                    FieldFinished = true;
                    return;
                }
                else
                {
                    field.Append(Line[Index]);
                    Index += 1;
                }
            }
        }

        // String builder holding the field
        private readonly StringBuilder field = new StringBuilder();

        // The regular expression used to find the next delimiter
        private readonly Regex delimiterRegex;

        // Chars that should be counted as space (and hence ignored if occurring before or after a delimiter
        private readonly string spaceChars;
    }
}