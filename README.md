# Upward

Upward is a "abandoned" versioned release service project that I wrote when I first learned C#

## API Paths

### POST `myproject.pkg.example.com/create`

Create a new version on a new/existing branch

Request Headers required:
* Content-Type: `application/json`
* Authorization: `Beader my-api-key`

Example body:

```json
{
  "version": "2.2.9",
  "branch": "beta"
}
```

Expected response:
200
```json
{
  "url": "https://myproject.pkg.example.com/:currentbranch/:currentversion/:thefilename",
  "created": "A date",
  "branch": "currentbranch",
  "version": "version"
}
```

This create a branch called `beta` if it doesn't exist and create version 2.2.9. Now you just need to PUT files


### PUT: `myproject.pkg.example.com/:branch/:version/:filename`

Add file to a version in a branch

Request Headers required:

* Content-Type: `anycontent/type`
* Authorization: `Bearer my-api-key`

Expected response:
200
```json
{
  "url": "https://myproject.pkg.example.com/:currentbranch/:currentversion/:thefilename",
  "created": "A date",
  "branch": "currentbranch",
  "version": "version"
}
```

### GET: `myproject.pkg.example.com/:branch/:version/:filename`

Download a file in a specified branch and version

Request Headers required?:

Authorization required only if project is set to private
* Authorization: `Bearer my-api-key`

Expected response: 200 <file content>


### DELETE: `myproject.pkg.example.com/:branch/:version`

Delete version including the files in the version

Request Headers required:

* Authorization: `Bearer my-api-key`

Expected response: 200 Empty


### DELETE `myproject.pkg.example.com/:branch/:version/:filename`

Delete a file in a specified branch and version

Request Headers required:

* Authorization: `Bearer my-api-key`

Expected response: 200 Empty

## API errors

### 400

```json
{
  "code": "AErrorCode",
  "message": "Message explaining the rror"
}
```
