import json
from collections import defaultdict

header_str = """<?xml version="1.0" encoding="utf-8" ?>
<Patch>"""
tmp_core = """
  <Operation Class="PatchOperationAdd">
    <xpath>Defs/ThingDef[defName="{bench}"]/comps/li[@Class="CompProperties_AffectedByFacilities"]/linkableFacilities</xpath>
    <value>
      {items}
    </value>
  </Operation>
"""
tmp_mod = """
  <Operation Class="PatchOperationFindMod">
    <mods>
      <li>{modname}</li>
    </mods>
    <match Class="PatchOperationAdd">
      <xpath>Defs/ThingDef[defName="{bench}"]/comps/li[@Class="CompProperties_AffectedByFacilities"]/linkableFacilities</xpath>
      <value>
        {items}
      </value>
    </match>
  </Operation>
"""
footer_str = """
</Patch>
"""
with open(r"linkable_patches.xml", mode='w') as lkb_f:
    lkb_f.write(header_str)
    categories = defaultdict(list)
    with open(r"linkables.txt") as linkables:
        for line in linkables:
            category = line.split(' - ')[1].strip()
            item = line.split(' - ')[0].strip()
            categories[category].append(item)

    with open(r"benches.txt") as benches:
        for line in benches:
            bench = line.split(' - ')[0]
            mod = ' - '.join(line.split(' - ')[1:-1])
            category = line.rsplit(' - ')[-1].strip()

            if category in categories.keys(): 
                if mod == "Core":
                    spaces = ' ' * 6
                    items = str(categories[category]).replace('[', '<li>').replace(', ', f'</li>\n{spaces}<li>').replace(']', '</li>').replace('\'', "")
                    lkb_f.write(tmp_core.format(bench=bench, items=items))
                else:
                    spaces = ' ' * 8
                    items = str(categories[category]).replace('[', '<li>').replace(', ', f'</li>\n{spaces}<li>').replace(']', '</li>').replace('\'', "")
                    lkb_f.write(tmp_mod.format(bench=bench, modname=mod, items=items))

    lkb_f.write(footer_str)