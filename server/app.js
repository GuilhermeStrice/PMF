const express = require('express');
const app = express();
const port = 3000;
const fs = require('fs');

const mariadb = require('mariadb');
const pool = mariadb.createPool({user: "root", database: "pmf"})

app.use(express.static('public'))

app.get('/package/:id', async (req, res) =>
{
    var con = await pool.getConnection()
    var pkg = (await con.query("SELECT id_package, ID, Type, Name, Description FROM package WHERE BINARY ID = BINARY ? LIMIT 1", [req.params.id]))
    if (pkg.length === 1)
    {
        pkg = pkg[0]
        var assets = await con.query("SELECT id_asset, Version, SdkVersion, Checksum, FileName, Url FROM asset_table WHERE id_package = ?", [package["id_package"]])

        // don't need this anymore
        delete pkg["id_package"]

        if (assets.length > 0)
        {
            for (var i = 0; i < assets.length; i++)
            {
                var dependencies = await con.query("SELECT ID, Checksum, FileName, Url FROM dependency WHERE id_asset = ?", [assets[i]["id_asset"]])

                // don't need this anymore
                delete assets[i]["id_asset"]

                assets[i]["Dependencies"] = dependencies
            }

            pkg["Assets"] = assets
        }
    }

    res.json(pkg)
});

app.listen(port, () => console.log(`http://localhost:${port}`));