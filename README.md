### APIs

<details>
<summary>Return Value Spec</summary>
```
{
    "success": <true|false>,
    "payload": <data if exists>,
    "message": <reason it success or failed>,
}
```
</details>
<h3>User</h3>
<details>
 <summary><code>POST</code> <code><b>/</b></code> <code>/User/Login</code></summary>
data:

```
{
    "Name": <name>,
    "Pwd": <password>,
}
```

return:

```
{
    "success": <is login success>,
    "payload": <tokenstring if login success>,
    "message": <reasons>,
}
```

</details>
<details>
 <summary><code>POST</code> <code><b>/</b></code> <code>/User/CheckDuplicatedName</code></summary>
data:

```
{
    "Name": <name>
}
```

return:

```
{
    "success": <is name Not duplicated>,
    "message": <reasons>,
}
```

</details>

<details>
 <summary><code>POST</code> <code><b>/</b></code> <code>/post</code></summary>
data:

```
{
    "Title":<title>,
    "Content":<content>,
    "NewTags":[
        {
            "Name":<a tag name>
        },
        {
            "Name":<another tag name>
        },
        ...
    ]
}
```

return:

```
{
    "success": <is add post success>,
    "message": <reasons>,
}
```

</details>
