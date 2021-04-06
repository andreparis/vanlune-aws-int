$StackName = "vanlune-aws-int"

rm -r C:\Projects\vanlune\vanlune-aws-int\Aws.Int.Application\bin\Release\netcoreapp3.1\publish

dotnet publish C:\Projects\vanlune\vanlune-aws-int -c release

$7zipPath = "$env:ProgramFiles\7-Zip\7z.exe"
if (-not (Test-Path -Path $7zipPath -PathType Leaf)) {
    throw "7 zip file '$7zipPath' not found"
}
Set-Alias 7zip $7zipPath
$Source = "C:\Projects\vanlune\vanlune-aws-int\Aws.Int.Application\bin\Release\netcoreapp3.1\publish\*"
$Target = "C:\Projects\vanlune\vanlune-aws-int\Aws.Int.Application\bin\Release\netcoreapp3.1\publish\Aws.Int.zip"

7zip a -mx=9 $Target $Source

aws s3 cp C:\Projects\vanlune\vanlune-aws-int\Aws.Int.Application\template-aws-int.yaml s3://vanlune-bin-dev
aws s3 cp C:\Projects\vanlune\vanlune-aws-int\Aws.Int.Application\bin\Release\netcoreapp3.1\publish\Aws.Int.zip s3://vanlune-bin-dev

$exists = aws cloudformation describe-stacks --stack-name $StackName
if ($exists)
{
	aws cloudformation  update-stack --stack-name $StackName --template-url https://vanlune-bin-dev.s3.amazonaws.com/template-aws-int.yaml --capabilities CAPABILITY_IAM CAPABILITY_AUTO_EXPAND
}
else
{
	aws cloudformation create-stack  --stack-name $StackName --template-url https://vanlune-bin-dev.s3.amazonaws.com/template-aws-int.yaml --capabilities CAPABILITY_IAM CAPABILITY_AUTO_EXPAND
}
aws cloudformation wait stack-update-complete --stack-name $StackName

aws lambda update-function-code --function-name vanlune-aws-int-sns    --s3-bucket vanlune-bin-dev --s3-key Aws.Int.zip
aws lambda update-function-code --function-name vanlune-aws-int-secret --s3-bucket vanlune-bin-dev --s3-key Aws.Int.zip