with open('ids.txt') as input, open('idlist.txt', 'w') as output:
    for line in input:
        output.write(f"<li>{line.split(' - ')[-1].strip()}</li>\n")
