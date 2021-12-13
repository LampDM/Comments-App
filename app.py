from flask import Flask, render_template, request
import pymysql
app = Flask(__name__)

@app.route('/')
def index():
    return render_template("comment.html")

@app.route('/getComments',methods=['GET'])
def getComments():
    ret = ""
    connection = pymysql.connect(user='root', passwd='supersecret', host='db-sql', database='company')
    cursor = connection.cursor()
    query = ("select * from comments")
    cursor.execute(query)
    for item in cursor:
        ret += item[0] + "<br>"
    return ret

@app.route('/addComment', methods=['POST'])
def addComment():
    comment = (request.form['projectFilePath'])
    print(comment)

    #Insert into mysql
    try:
        connection = pymysql.connect(user='root', passwd='supersecret', host='172.17.0.1', database='company')
        cursor = connection.cursor()
        query = ("Insert into comments(comment) values({})".format("'"+comment+"'"))
        cursor.execute(query)
        connection.commit()

    except Exception as e:
        print("Error while connecting to MySQL", e)
        return e

    return "Comment posted!"

app.run(host='0.0.0.0', port=81)