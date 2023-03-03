################################################################################
# asmdef のファイル名や name の値が正しいかどうかをチェックします。
################################################################################

import glob
import os
import logging
import coloredlogs
import json

# ロギング設定
logger = logging.getLogger(__name__)
coloredlogs.install(fmt="[%(levelname)s] %(message)s")

# 定数
PWD = os.path.dirname(os.path.abspath(__file__))
PATHS = {"unitytools_dir": os.path.abspath(f"{PWD}/../Assets/UnityTools")}
BASE_NAMESPACE = "GigaCreation.Tools"


def main() -> None:
    """メイン関数です。"""

    for path in glob.glob(f"{PATHS['unitytools_dir']}/**/*.asmdef", recursive=True):
        asmdef_name = os.path.splitext(os.path.basename(path))[0]

        expected_namespace = (
            BASE_NAMESPACE
            + "."
            + os.path.dirname(path)
            .replace(PATHS["unitytools_dir"], "")
            .replace("Runtime", "")
            .strip("/")
            .replace("/", ".")
            .replace("_", ".")
        )

        with open(path, "r", encoding="utf_8", newline="\n") as file:
            asmdef_json = json.load(file)

            if asmdef_name != expected_namespace:
                logging.warning(f"asmdef のファイル名と期待される名前空間が一致しません：{asmdef_name}")

            if asmdef_name != asmdef_json["name"]:
                logging.info(f"asmdef のファイル名と name の値が一致しません：{asmdef_name}")


main()
