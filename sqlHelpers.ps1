function ImportSource() {

    Import-Csv  "C:\Users\cernu\Documents\Datasets\remotelinksource.csv" -Header Id, FeedCode, Url, SourceUrl, IconUmb, Enabled |
    ForEach-Object {
        $imageQuery = "(SELECT Id FROM NV_BinaryData_Static WHERE Notes = '$($_.FeedCode)')"
        @"
        insert into nv_remotelinksource (FeedCode,Url,SourceUrl,Enabled, ImageId) values ('$($_.FeedCode)','$($_.url)','$($_.SourceUrl)',$($_.Enabled), $imageQuery);
"@
    } | Set-Clipboard
}

function ImportImages() {

    Get-ChildItem -r -i *.jpg "C:\Users\cernu\Downloads\media\Media" |
    ForEach-Object {
@"
        INSERT INTO NV_BinaryData_Static (Notes, Data)
        SELECT '$($_.Name.replace(".jpg",""))', BulkColumn
        FROM Openrowset( Bulk '$($_.Fullname)', Single_Blob) as image;

"@
    } | Set-Clipboard
}

ImportSource