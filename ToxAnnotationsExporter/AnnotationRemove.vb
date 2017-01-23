Imports System.Data.SQLite
Imports System.IO

Public Module AnnotationRemove
    Private ReadOnly DATABASE_FILE_RELATIVE_PATH As [String] = "Sony_Reader\database\books.db"
    Private readerStorageLocations As New List(Of StorageInfo)()


    Sub remove_annotation(book As BookInfo, location As List(Of StorageInfo))

        For Each storageLocation As StorageInfo In location
            If True = [String].IsNullOrEmpty(storageLocation.BasePath) Or storageLocation.BasePath = "\" Then
                Continue For
            End If

            Using dbConnection As New SQLiteConnection()
                Try
                    dbConnection.ConnectionString = [String].Format("Data Source={0};FailIfMissing=True;", Path.Combine(storageLocation.BasePath, DATABASE_FILE_RELATIVE_PATH))
                    dbConnection.Open()

                    Dim query = [String].Format("DELETE FROM annotation WHERE content_id = {0}", book.BookIdentification)
                    Dim dbCommand As SQLiteCommand = New SQLiteCommand(query, dbConnection)
                    dbCommand.ExecuteReader()
                Finally
                    dbConnection.Close()
                    dbConnection.Dispose()
                End Try
            End Using
        Next
    End Sub
End Module
